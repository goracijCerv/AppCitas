using CloudinaryDotNet.Actions;

namespace Apcitas.WebService.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile photofile);
    Task<DeletionResult> DeletePhotoAsync(string publicid);
}
