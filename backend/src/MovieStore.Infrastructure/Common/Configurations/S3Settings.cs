using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

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