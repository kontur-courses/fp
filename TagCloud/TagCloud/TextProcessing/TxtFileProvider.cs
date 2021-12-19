using System.IO;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public class TxtFileProvider : IFileProvider
    {
        public Result<string> GetTxtFilePath(string path)
        {
            return !File.Exists(path) 
                ? Result.Fail<string>($"Файл по пути {path} не был найден")
                : Result.Ok(path);
        }
    }
}