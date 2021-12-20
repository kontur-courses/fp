using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ResultMonad;
using TagsCloudVisualization.WordsProvider.FileReader;

namespace TagsCloudVisualization.WordsProvider
{
    internal class FileReadService : IFileReadService
    {
        private const string WordsSplitPattern = @"\W+";
        private readonly Regex wordsSplit = new Regex(WordsSplitPattern);
        private readonly string path;
        private readonly IEnumerable<IWordsReader> readers;
        private readonly string extension;

        public FileReadService(string path, IEnumerable<IWordsReader> readers)
        {
            this.path = path;
            this.readers = readers;
            extension = Path.GetExtension(path);
        }

        public Result<IEnumerable<string>> GetFileContent()
        {
            return path
                .AsResult()
                .Validate(File.Exists, $"File {path} not found")
                .Then(_ => readers.FirstOrDefault(reader => reader.IsSupportedFileExtension(extension)))
                .Validate(reader => reader != null, $"Unsupported file extension: {extension}")
                .Then(reader => reader.GetFileContent(path))
                .Then(x => x.SelectMany(y => wordsSplit.Split(y)));
        }
    }
}