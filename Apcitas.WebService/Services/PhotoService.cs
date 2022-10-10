using Apcitas.WebService.Helpers;
using Apcitas.WebService.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Apcitas.WebService.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService( IOptions<CloudinarySettings> config)
    {
        var acc = new Account
        (
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile photofile)
    {
        var uploadRes = new ImageUploadResult();

        if(photofile.Length > 0)
        {
            using var stream = photofile.OpenReadStream();
            var uploadParm = new ImageUploadParams
            {
                File = new FileDescription(photofile.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };
            uploadRes = await _cloudinary.UploadAsync(uploadParm);
        }
        return uploadRes;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicid)
    {
        var deleteParms = new DeletionParams(publicid);

        var result = await _cloudinary.DestroyAsync(deleteParms);
        return result;
    }
}
