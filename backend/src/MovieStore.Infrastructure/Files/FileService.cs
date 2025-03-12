using MovieStore.Application.Common.Interfaces;

namespace MovieStore.Infrastructure.Files;

public class FileService() : IFileService
{
    private const string ImagesFolder = "images";
    
    public async Task<string> UploadFileAsync(Stream file, string webRootPath, string fileExtension)
    {
        string fileName = Guid.NewGuid() + fileExtension;
        string filePath = Path.Combine(ImagesFolder, fileName);
        string uploadPath = Path.Combine(webRootPath, filePath);
        
        await using var fileStreamOutput = new FileStream(uploadPath, FileMode.Create);
        await file.CopyToAsync(fileStreamOutput);

        return filePath;
    }
}