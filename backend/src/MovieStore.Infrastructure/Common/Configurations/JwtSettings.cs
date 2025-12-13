using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

public class JwtSettings
{
    [Required(AllowEmptyStrings = false)]
    public string Secret { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Issuer { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Audience { get; set; }
    [Required]
    public TimeSpan JwtTokenLifetime { get; set; }
    [Required]
    public TimeSpan RefreshTokenLifetime { get; set; }
}