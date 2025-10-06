using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Configuration;

public class FileStorageSettings
{
    [Required(AllowEmptyStrings = false)]
    public string FolderPath { get; set; }
}