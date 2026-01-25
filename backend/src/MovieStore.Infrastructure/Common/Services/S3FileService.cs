using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Infrastructure.Common.Configurations;

namespace MovieStore.Infrastructure.Common.Services;

public class S3FileService(IAmazonS3 s3Client, IOptions<S3Settings> s3Options) : IFileService
{
    private readonly S3Settings _s3Settings = s3Options.Value;
    
    public async Task<string> UploadFileAsync(Stream file, string fileExtension)
    {
        var fileFullName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{fileExtension}";
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = file,
            Key = fileFullName,
            BucketName = _s3Settings.BucketName
        };
        using var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return fileFullName;
    }

    public async Task DeleteFileAsync(string path)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = path
        };
        
        await s3Client.DeleteObjectAsync(deleteRequest);
    }
}