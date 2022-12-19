using System.IO;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class FileValidator : IFileValidator
    {
        public Result<string> VerifyFileExistence(string path)
        {
            if (path == null)
            {
                return Result.Fail<string>("Null path");
            }

            if (!File.Exists(path))
            {
                return Result.Fail<string>("File was not found");
            }

            return Result.Ok(path);
        }
    }
}