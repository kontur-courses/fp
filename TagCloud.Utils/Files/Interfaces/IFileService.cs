using TagCloud.Utils.ResultPattern;

namespace TagCloud.Utils.Files.Interfaces;

public interface IFileService
{
    public Result<None> CreateDirectory(string path);
    public Result<None> CheckExistance(string path);
}