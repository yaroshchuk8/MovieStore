using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Configuration;

internal class DbSettings
{
    [Required(AllowEmptyStrings = false)]
    public string ConnectionString { get; set; }    
}