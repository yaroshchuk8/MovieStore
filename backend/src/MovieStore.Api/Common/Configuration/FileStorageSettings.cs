using System.ComponentModel.DataAnnotations;

namespace MovieStore.Api.Common.Configuration;

public class FileStorageSettings
{
    [Required(AllowEmptyStrings = false)]
    public string FolderPath { get; set; }
}