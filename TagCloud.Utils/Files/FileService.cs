using TagCloud.Utils.Files.Interfaces;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.Utils.Files;

public class FileService : IFileService
{
    public Result<None> CreateDirectory(string path)
    {
        return Result.OfAction(() =>
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        });
    }

    public Result<None> CheckExistance(string path)
    {
        return Result.OfAction(() =>
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"По пути {path} не найдено файла");
        });
    }
}