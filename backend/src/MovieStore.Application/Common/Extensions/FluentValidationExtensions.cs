using FileSignatures;
using FileSignatures.Formats;
using FluentValidation;
using MovieStore.Application.Common.DTOs;

namespace MovieStore.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    private const string JpgExtension = ".jpg";
    private const string JpegExtension = ".jpeg";
    private const string PngExtension = ".png";
    private const string Mp4Extension = ".mp4";

    // Map extensions to the "FileFormat" type the library should detect
    private static readonly Dictionary<string, Type> SupportedExtensions = new()
    {
        { JpgExtension, typeof(Jpeg) },
        { JpegExtension, typeof(Jpeg) },
        { PngExtension, typeof(Png) },
        { Mp4Extension, typeof(MP4) }
    };
    
    extension<T>(IRuleBuilder<T, FileDescriptor?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, FileDescriptor?> MustBeValidImage(IFileFormatInspector inspector)
        {
            var allowedExtensions = new[] { JpgExtension, JpegExtension, PngExtension };
            
            return ruleBuilder.ValidateRequiredFileAgainst(inspector, allowedExtensions);
        }

        public IRuleBuilderOptions<T, FileDescriptor?> MustBeValidVideo(IFileFormatInspector inspector)
        {
            var allowedExtensions = new[] { Mp4Extension };
            
            return ruleBuilder.ValidateRequiredFileAgainst(inspector, allowedExtensions);
        }
        
        public IRuleBuilderOptions<T, FileDescriptor?> MustBeValidImageOrVideo(IFileFormatInspector inspector)
        {
            var allowedExtensions = new[] { JpgExtension, JpegExtension, PngExtension, Mp4Extension };
            
            return ruleBuilder.ValidateRequiredFileAgainst(inspector, allowedExtensions);
        }

        private IRuleBuilderOptions<T, FileDescriptor?> ValidateRequiredFileAgainst(
            IFileFormatInspector inspector, 
            string[] allowedExtensions)
        {
            return ruleBuilder
                .NotNull().WithMessage("File is required.")
                .Must(file =>
                {
                    var ext = file!.Extension.ToLowerInvariant();

                    // Check 1: Is the extension allowed for THIS specific field?
                    if (!allowedExtensions.Contains(ext)) return false;

                    // Check 2: Does the extension exist in our global map?
                    if (!SupportedExtensions.TryGetValue(ext, out var expectedFormatType)) return false;

                    // Check 3: Inspect Magic Numbers
                    if (file.Content.CanSeek) file.Content.Position = 0;
                    var detectedFormat = inspector.DetermineFileFormat(file.Content);
                    if (file.Content.CanSeek) file.Content.Position = 0;

                    // Check 4: Cross-reference Content with Extension
                    return detectedFormat != null && expectedFormatType.IsInstanceOfType(detectedFormat);
                })
                .WithMessage($"Invalid file format. Allowed: {string.Join(", ", allowedExtensions)}.");
        }
    }
}