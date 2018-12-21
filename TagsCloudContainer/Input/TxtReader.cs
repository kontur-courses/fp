using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Input
{
    public class TxtReader : IFileReader
    {
        public Result<string> Read(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<string>("Text file not found");

            return Result.Ok(File.ReadAllText(path));
        }
    }
}