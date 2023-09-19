namespace BakaMangaAPI.Services.Media;

public interface IMediaManager
{
    /// <summary>
    /// Upload the image file to an image storage and return the full
    /// url to retrieve the image.
    /// </summary>
    /// <param name="imageFile">the image file get from form.</param>
    /// <param name="imageId">the image id that will be included as the name of
    /// the file that will be stored in the storage.</param>
    /// <param name="imageType">the image type to sort it into the
    /// corresponding folder</param>
    /// <returns>the full url string to the image.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the imageFile or imageId is null.
    /// </exception>
    Task<string> UploadImageAsync(IFormFile imageFile, string imageId, ImageType imageType);

    /// <summary>
    /// Delete the image in the image storage.
    /// </summary>
    /// <param name="imageId">the image path</param>
    /// <exception cref=""></exception>
    Task DeleteImageAsync(string imagePath);
}

public enum ImageType
{
    Avatar, Banner, Cover, Page
}
