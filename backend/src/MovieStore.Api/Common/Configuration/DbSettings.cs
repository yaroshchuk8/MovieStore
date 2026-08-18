using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

public class DbSettings
{
    [Required(AllowEmptyStrings = false)]
    public string ConnectionString { get; set; }
}