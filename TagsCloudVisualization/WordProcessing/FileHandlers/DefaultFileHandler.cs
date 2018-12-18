using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization.WordProcessing.FileHandlers
{
    public class DefaultFileHandler : IFileHandler
    {
        public string PathToFile { get; }

        public DefaultFileHandler(string pathToFile)
        {
            PathToFile = pathToFile;
        }
        public Result<IEnumerable<string>> ReadFile()
        {
            return Result.Ok(new List<string>())
                .Then(ReadWords, "Could not read file.")
                .Then(words => (IEnumerable<string>)words);
        }

        private void ReadWords(List<string> words)
        {
            using (var streamReader = new StreamReader(PathToFile))
            {
                while (!streamReader.EndOfStream)
                    words.Add(streamReader.ReadLine());
            }
        }
    }
}