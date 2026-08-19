using System.ComponentModel.DataAnnotations;

namespace MovieStore.Api.Common.Configuration;

public class S3Settings
{
    [Required(AllowEmptyStrings = false)]
    public string Endpoint { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string BucketName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string AccessKey { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string SecretKey { get; set; }
}