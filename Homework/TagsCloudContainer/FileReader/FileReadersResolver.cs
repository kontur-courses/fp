using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudContainer.FileReader
{
    public class FileReadersResolver : IResolver<string, IFileReader>
    {
        private readonly Dictionary<string, IFileReader> fileReadersResolver;


        public FileReadersResolver(params IFileReader[] readers)
        {
            fileReadersResolver = readers.ToDictionary(x => x.Extension);
        }

        public Result<IFileReader> Get(string path)
        {
            return CheckNullOrEmpty(path)
                .Then(GetFileReader);
        }

        private Result<string> CheckNullOrEmpty(string path)
        {
            return !string.IsNullOrEmpty(path)
                ? path
                : Result.Fail<string>("Path is null or empty");
        }

        private Result<IFileReader> GetFileReader(string path)
        {
            var ext = Path.GetExtension(path);
            if (fileReadersResolver.ContainsKey(ext))
                return Result.Ok(fileReadersResolver[ext]);
            return Result.Fail<IFileReader>($"Format {ext} is not supported");
        }
    }
}