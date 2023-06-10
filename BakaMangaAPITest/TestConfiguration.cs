namespace BakaMangaAPITest;

public static class TestConfiguration
{
    public static string DbConnectionString { get; }

    public static string ImageStorageUrl { get; }

    static TestConfiguration()
    {
        DotNetEnv.Env.TraversePath().Load();
        DbConnectionString = DotNetEnv.Env.GetString("TestDatabase");
        ImageStorageUrl = DotNetEnv.Env.GetString("TestImageStorageUrl");
    }
}
