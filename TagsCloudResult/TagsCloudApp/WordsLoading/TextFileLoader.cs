using System.Collections.Generic;
using System.IO;
using TagsCloudContainer.Results;

namespace TagsCloudApp.WordsLoading
{
    public abstract class TextFileLoader : IFileTextLoader
    {
        public abstract IEnumerable<FileType> SupportedTypes { get; }

        public Result<string> LoadText(string filename)
        {
            return File.Exists(filename)
                ? LoadTextFromExistingFile(filename)
                : Result.Fail<string>($"File not exist at: {filename}.");
        }

        protected abstract Result<string> LoadTextFromExistingFile(string filename);
    }
}