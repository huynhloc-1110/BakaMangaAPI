namespace BakaMangaAPI.Services;

public class LocalMediaManager : IMediaManager
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private const string _rootImageFolderName = "img";

    public LocalMediaManager(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> UploadImage(IFormFile imageFile, string imageId, ImageType imageType)
    {
        // validate
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }
        if (imageId == null)
        {
            throw new ArgumentNullException(nameof(imageId));
        }

        var folderPath = PrepareDirectory(imageType);
        var imageName = PrepareImageName(imageFile.FileName, imageId);
        var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath, imageName);

        // create file in local
        using (var fileStream = new FileStream(uploadPath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return Path.Combine("https://localhost:7036", folderPath, imageName);
    }

    private string PrepareDirectory(ImageType imageType)
    {
        string imageDir = Path.Combine(_rootImageFolderName, imageType switch
        {
            ImageType.Avatar => "avatars",
            ImageType.Cover => "covers",
            ImageType.Page => "pages",
            _ => throw new ArgumentException("Invalid image type")
        });

        // create folder if not exist
        string uploadDir = Path.Combine(_rootImageFolderName, imageDir);
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        return imageDir;
    }

    private string PrepareImageName(string imageName, string imageId)
    {
        var imageExtension = Path.GetExtension(imageName);
        var imageNameWithExtension = Path.ChangeExtension(imageId,
            imageExtension);
        return imageNameWithExtension;
    }
}
