using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

internal class RefreshTokenSettings
{
    [Required]
    public TimeSpan RefreshTokenLifetime { get; set; }
}