using BakaMangaAPI.Services;
using Microsoft.AspNetCore.Http;

namespace BakaMangaAPITest.Mocks;

public class MockMediaManager : IMediaManager
{
    public Task<string> UploadImage(IFormFile imageFile, string imageId)
    {
        return Task.FromResult($"{TestConfiguration.ImageStorageUrl}/{imageId}.jpg");
    }
}
