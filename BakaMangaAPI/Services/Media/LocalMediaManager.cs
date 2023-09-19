namespace BakaMangaAPI.Services.Media;

public class LocalMediaManager : IMediaManager
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IConfiguration _configuration;
    private const string _rootImageFolderName = "img";

    public LocalMediaManager(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
    {
        _hostingEnvironment = hostingEnvironment;
        _configuration = configuration;
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile, string imageId, ImageType imageType)
    {
        // validate image file and id not null
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }
        if (imageId == null)
        {
            throw new ArgumentNullException(nameof(imageId));
        }

        // prepare path
        var imageExtension = Path.GetExtension(imageFile.FileName);
        var imageFullName = Path.ChangeExtension(imageId, imageExtension);
        var imageSubFolderName = imageType.ToString().ToLower() + "s";
        var imagePath = Path.Combine(
            _hostingEnvironment.WebRootPath,
            _rootImageFolderName,
            imageSubFolderName,
            imageFullName);

        // ensure directory exists
        Directory.CreateDirectory(Directory.GetParent(imagePath)!.FullName);

        // copy form file to the image path
        using var fileStream = new FileStream(imagePath, FileMode.Create);
        await imageFile.CopyToAsync(fileStream);

        // construct and return url
        var serverUri = _configuration["Jwt:ValidIssuer"];
        return $"{serverUri}/{_rootImageFolderName}/{imageSubFolderName}/{imageFullName}";
    }

    public Task DeleteImageAsync(string imagePath)
    {
        var uri = new Uri(imagePath);
        var relativeFilePath = Path.Combine(
            uri.AbsolutePath.Split("/", StringSplitOptions.RemoveEmptyEntries));
        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, relativeFilePath);

        File.Delete(filePath);
        return Task.CompletedTask;
    }
}
