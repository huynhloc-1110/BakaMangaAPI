namespace BakaMangaAPI.Services;

public class LocalMediaManager : IMediaManager
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private const string _imageDirectoryName = "img";

    public LocalMediaManager(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> UploadImage(IFormFile imageFile, string imageId)
    {
        if (imageFile == null || imageId == null)
        {
            throw new ArgumentNullException();
        }

        var uploadDir = PrepareDirectory();

        // prepare file path
        var imageExtension = Path.GetExtension(imageFile.FileName);
        var imageNameWithExtension = Path.ChangeExtension(imageId,
            imageExtension);
        var imagePath = Path.Combine(uploadDir, imageNameWithExtension);

        // create file in local
        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return PrepareImageUrl(imageNameWithExtension);
    }

    private string PrepareDirectory()
    {
        string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath,
            _imageDirectoryName);
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }
        return uploadDir;
    }

    private string PrepareImageUrl(string imageNameWithExtension)
    {
        return $"https://localhost:7036/{_imageDirectoryName}/{imageNameWithExtension}";
    }
}
