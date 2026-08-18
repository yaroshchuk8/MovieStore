using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace MovieStore.Application.Common.Extensions;

public static class ConfigurationExtensions
{
    extension(IConfiguration configuration)
    {
        public void ValidateRequiredSection<T>(string sectionName)
            where T : class, new()
        {
            var configSection = configuration.GetRequiredSection(sectionName);
            var configObject = new T();
            configSection.Bind(configObject);
        
            var context = new ValidationContext(configObject);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(configObject, context, results, true))
            {
                return;
            }
        
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new InvalidOperationException($"Invalid configuration for section '{sectionName}': {errors}");
        }

        public IConfigurationSection GetAndValidateRequiredSection<T>(string sectionName)
            where T : class, new()
        {
            var configSection = configuration.GetRequiredSection(sectionName);
            var configObject = new T();
            configSection.Bind(configObject);
        
            var context = new ValidationContext(configObject);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(configObject, context, results, true))
            {
                return configSection;
            }
        
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new InvalidOperationException($"Invalid configuration for section '{sectionName}': {errors}");
        }
    }
}