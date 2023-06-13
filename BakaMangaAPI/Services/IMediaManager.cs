namespace BakaMangaAPI.Services;

public interface IMediaManager
{
    /// <summary>
    /// Upload the image file to an image storage and return the full
    /// url to retrieve the image.
    /// </summary>
    /// <param name="imageFile">the image file get from form.</param>
    /// <param name="imageId">the image id that will be included as the name of
    /// The file that will be stored in the storage.</param>
    /// <returns>the full url string to the image.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the imageFile or imageId is null.
    /// </exception>
    Task<string> UploadImage(IFormFile imageFile, string imageId);
}
