using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

public class RefreshTokenSettings
{
    [Required]
    public TimeSpan RefreshTokenLifetime { get; set; }
}