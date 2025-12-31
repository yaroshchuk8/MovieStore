using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

public class FileStorageSettings
{
    [Required(AllowEmptyStrings = false)]
    public string FolderPath { get; set; }
}