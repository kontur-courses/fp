using TagCloudContainer.Core;

namespace TagCloudContainer.Utils;

public static class PathAssistant
{
    public static Result<string> GetMainDirectoryPath()
    {
        var directoryParentResult = GetCurrentDirectoryParent();
        if (directoryParentResult.IsSuccess)
            return directoryParentResult.Value;
        return Result.Fail<string>(directoryParentResult.Error);
    }


    public static Result<string> GetFullFilePath(string fileName)
    {
        var directoryParentResult = GetCurrentDirectoryParent();
        if (directoryParentResult.IsSuccess)
            return Path.Combine(directoryParentResult.Value, fileName);
        return Result.Fail<string>(directoryParentResult.Error);
    }

    private static Result<string> GetCurrentDirectoryParent()
    {
        var currentDirectoryParent = Directory.GetParent(Directory.GetCurrentDirectory());
        if (currentDirectoryParent != null)
            return Result.Ok(Path.Combine(currentDirectoryParent.FullName, currentDirectoryParent.Parent.Parent.Parent.FullName));
        return Result.Fail<string>("Parent of current directory is null");
    }
}