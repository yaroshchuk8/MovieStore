using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

internal class DbSettings
{
    [Required(AllowEmptyStrings = false)]
    public string ConnectionString { get; set; }
}