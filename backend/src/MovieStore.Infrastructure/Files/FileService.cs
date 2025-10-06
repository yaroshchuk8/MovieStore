using MovieStore.Application.Common.Interfaces;

namespace MovieStore.Infrastructure.Files;

public class FileService : IFileService
{
    private const string ImagesFolder = "images";
    
    public async Task<string> UploadFileAsync(Stream file, string webRootPath, string fileExtension)
    {
        var fileName = Guid.NewGuid() + fileExtension;
        var filePath = Path.Combine(ImagesFolder, fileName);
        var uploadPath = Path.Combine(webRootPath, filePath);
        
        await using var fileStreamOutput = new FileStream(uploadPath, FileMode.Create);
        await file.CopyToAsync(fileStreamOutput);

        return filePath;
    }
}