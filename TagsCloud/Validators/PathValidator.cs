using System.Security;
using TagsCloud.Extensions;
using TagsCloud.Results;

namespace TagsCloud.Validators;

public static class PathValidator
{
    public static Result<bool> ValidateFile(string filePath)
    {
        return !File.Exists(filePath)
            ? ResultExtensions.Fail<bool>($"File {filePath} doesn't exist!")
            : CheckFileAvailability(filePath);
    }

    public static Result<bool> ValidateDirectory(string directoryPath)
    {
        return !Directory.Exists(directoryPath)
            ? ResultExtensions.Fail<bool>($"Directory {directoryPath} doesn't exist!")
            : CheckDirectoryAvailability(directoryPath);
    }

    private static Result<bool> CheckFileAvailability(string filePath)
    {
        try
        {
            _ = File.ReadLines(filePath);
        }
        catch (SecurityException)
        {
            return ResultExtensions.Fail<bool>("Insufficient access rights!");
        }
        catch (UnauthorizedAccessException)
        {
            return ResultExtensions.Fail<bool>("Not supported operation or read-only file!");
        }
        catch (IOException)
        {
            return ResultExtensions.Fail<bool>("Unknown IO error!");
        }

        return ResultExtensions.Ok(true);
    }

    private static Result<bool> CheckDirectoryAvailability(string directoryPath)
    {
        try
        {
            using var fileStream = File.Create(
                Path.Combine(directoryPath, Path.GetRandomFileName()),
                1,
                FileOptions.DeleteOnClose);
        }
        catch (UnauthorizedAccessException)
        {
            return ResultExtensions.Fail<bool>("Insufficient access rights!");
        }
        catch (IOException)
        {
            return ResultExtensions.Fail<bool>("Unknown IO error!");
        }

        return ResultExtensions.Ok(true);
    }
}