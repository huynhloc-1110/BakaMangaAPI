
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BakaMangaAPI.Services.Media;

public class CloudinaryMediaManager : IMediaManager
{
    private readonly IConfiguration _configuration;
    private readonly Cloudinary _cloudinary;

    public CloudinaryMediaManager(IConfiguration configuration)
    {
        _configuration = configuration;
        _cloudinary = new Cloudinary(_configuration["CloudinaryUrl"]);
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        Uri uri = new(imagePath);
        string publicId = uri.Segments[uri.Segments.Length - 2]
            + Path.GetFileNameWithoutExtension(uri.Segments[uri.Segments.Length - 1]);

        var deletionParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deletionParams);
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile, string imageId, ImageType imageType)
    {
        var imageExtension = Path.GetExtension(imageFile.FileName);
        var imageFullName = Path.ChangeExtension(imageId, imageExtension);
        var imageFolderName = imageType.ToString().ToLower() + "s";

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(imageFullName, imageFile.OpenReadStream()),
            Folder = imageFolderName
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}
