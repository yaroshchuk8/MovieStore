using System.ComponentModel.DataAnnotations;

namespace MovieStore.Api.Configuration;

public class CorsSettings
{
    [Required(AllowEmptyStrings = false)]
    public string PolicyName { get; set; }
    [Required]
    public string[] AllowedOrigins { get; set; }
}