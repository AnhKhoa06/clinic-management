using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ClinicManagement.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string?> UploadAvatarAsync(IFormFile file, string fileName)
    {
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = "clinic-management/avatars",
            Transformation = new Transformation().Width(200).Height(200).Crop("fill")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }
}