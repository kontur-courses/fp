using ResultOf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TagsCloudContainer.Words;

namespace TagsCloudContainer
{
    public class TextFileWordsReader : IWordsReader
    {
        private readonly Result<string> filePath;

        public TextFileWordsReader(Result<string> filePath)
        {
            this.filePath = filePath;
        }

        public Result<string[]> ReadWords()
        {
            return filePath.Then(path =>
            {
                using (var fileStream = File.OpenRead(path))
                {
                    var array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    var textFromFile = Encoding.Default.GetString(array);
                    var separators = new string[] { Environment.NewLine };
                    return textFromFile.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToArray();
                }
            }).RefineError("File reading error");
        }

        public Result<HashSet<string>> ReadWordsInHashSet()
        {
            return ReadWords().Then((w) => new HashSet<string>(w)).RefineError("Read words in HashSet have error");
        }
    }
}
