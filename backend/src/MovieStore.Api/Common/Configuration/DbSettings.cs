using System.ComponentModel.DataAnnotations;

namespace MovieStore.Api.Common.Configuration;

public class DbSettings
{
    [Required(AllowEmptyStrings = false)]
    public string ConnectionString { get; set; }
}