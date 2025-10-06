using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace MovieStore.Application.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetAndValidateSection<T>(this IConfiguration configuration, string sectionName)
        where T : class, new()
    {
        var settings = configuration.GetSection(sectionName).Get<T>()
                       ?? throw new InvalidOperationException($"Missing configuration section '{sectionName}'.");

        var context = new ValidationContext(settings);
        var results = new List<ValidationResult>();
        
        if (!Validator.TryValidateObject(settings, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new InvalidOperationException($"Invalid configuration for section '{sectionName}': {errors}");
        }

        return settings;
    }
}