using System.ComponentModel.DataAnnotations;

namespace MovieStore.Api.Common.Configuration;

public class RefreshTokenSettings
{
    [Required]
    public TimeSpan RefreshTokenLifetime { get; set; }
}