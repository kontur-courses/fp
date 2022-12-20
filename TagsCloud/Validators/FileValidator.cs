using System.IO;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class FileValidator : IFileValidator
    {
        public ResultHandler<string> VerifyFileExistence(string path)
        {
            var handler = new ResultHandler<string>(path);

            if (path == null)
            {
                return handler.Fail("Null path");
            }

            if (!File.Exists(path))
            {
                return handler.Fail("File was not found");
            }

            return handler;
        }
    }
}