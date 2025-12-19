using System.ComponentModel.DataAnnotations;

namespace MovieStore.Infrastructure.Common.Configurations;

internal class FileStorageSettings
{
    [Required(AllowEmptyStrings = false)]
    public string FolderPath { get; set; }
}