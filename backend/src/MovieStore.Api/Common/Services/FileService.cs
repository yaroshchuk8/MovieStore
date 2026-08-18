using Microsoft.Extensions.Options;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Infrastructure.Common.Configurations;

namespace MovieStore.Infrastructure.Common.Services;

public class FileService(IOptions<FileStorageSettings> fileStorageOptions) : IFileService
{
    private readonly FileStorageSettings _fileStorageSettings = fileStorageOptions.Value;
    
    public async Task<string> UploadFileAsync(Stream file, string fileExtension)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        var uniqueId = Guid.NewGuid().ToString("N");
        var fileFullName = $"{timestamp}_{uniqueId}{fileExtension}";
        var uploadPath = Path.Combine(_fileStorageSettings.FolderPath, fileFullName);
      
        if (!Directory.Exists(_fileStorageSettings.FolderPath))
        {
            Directory.CreateDirectory(_fileStorageSettings.FolderPath);
        }
        
        await using var fileStreamOutput = new FileStream(uploadPath, FileMode.Create);
        await file.CopyToAsync(fileStreamOutput);

        return fileFullName;
    }
    
    public async Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_fileStorageSettings.FolderPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}