using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Infrastructure.Common.Configurations;

namespace MovieStore.Infrastructure.Common.Services;

public class S3Initializer(
    IAmazonS3 s3Client,
    IOptions<S3Settings> s3Options,
    ILogger<S3Initializer> logger)
    : IFileStorageInitializer
{
    private readonly S3Settings _s3Settings = s3Options.Value;
    
    public async Task InitializeAsync()
    {
        await CheckMinioHealthAsync();
        await EnsureCredentialsValidityAsync();
        await EnsureBucketExists();
    }

    private async Task CheckMinioHealthAsync()
    {
        var retryCount = 0;
        const int retryAttempts = 5;
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        
        while (retryCount < retryAttempts)
        {
            try
            {
                // MinIO returns 200 OK
                var response = await httpClient.GetAsync($"{_s3Settings.Endpoint}/minio/health/live");
                if (response.IsSuccessStatusCode) return;
            }
            catch (Exception)
            {
                retryCount++;
                logger.LogWarning($"MinIO is not reachable. Retry {retryCount}/{retryAttempts}.");
                await Task.Delay(2000);
            }
        }
        
        throw new Exception("MinIO not reachable.");
    }

    private async Task EnsureCredentialsValidityAsync()
    {
        try
        {
            // This forces the SDK to sign the request and MinIO to validate the signature
            await s3Client.ListBucketsAsync();
        }
        catch (AmazonS3Exception e) when (e.ErrorCode is "InvalidAccessKeyId" or "SignatureDoesNotMatch")
        {
            logger.LogError("S3 Authentication failed: Check your Access and Secret keys.");
            throw;
        }
    }
    
    private async Task EnsureBucketExists()
    {
        var bucketName = _s3Settings.BucketName;
        // Check/Create Bucket
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!bucketExists)
        {
            await s3Client.PutBucketAsync(new PutBucketRequest 
            { 
                BucketName = bucketName 
            });
        }
        
        var publicPolicy = $$"""
                             {
                                 "Version": "2012-10-17",		 	 	 
                                 "Statement": [
                                     {
                                         "Sid": "PublicReadGetObject",
                                         "Effect": "Allow",
                                         "Principal": "*",
                                         "Action": ["s3:GetObject"],
                                         "Resource": ["arn:aws:s3:::{{bucketName}}/*"]
                                     }
                                 ]
                             }
                             """;
       
        await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest 
        { 
            BucketName = bucketName, 
            Policy = publicPolicy 
        });
    }
}