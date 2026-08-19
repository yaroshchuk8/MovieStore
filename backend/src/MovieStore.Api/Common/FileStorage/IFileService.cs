namespace MovieStore.Api.Common.FileStorage;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream file, string fileExtension);
    Task DeleteFileAsync(string path);
}