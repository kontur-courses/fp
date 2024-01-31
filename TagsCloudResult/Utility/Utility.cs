namespace TagsCloudResult.Utility;

public static class Utility
{
    public static string GetAbsoluteFilePath(string fileName)
    {
        if (fileName.Contains('/') &&
            !Directory.Exists($"../../../../TagsCloudResult/{fileName[..fileName.LastIndexOf('/')]}"))
            Directory.CreateDirectory($"../../../../TagsCloudResult/{fileName[..fileName.LastIndexOf('/')]}");
        return $"../../../../TagsCloudResult/{fileName}";
    }
}