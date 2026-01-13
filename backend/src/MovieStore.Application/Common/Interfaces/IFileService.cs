namespace MovieStore.Application.Common.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream file, string fileExtension);
}