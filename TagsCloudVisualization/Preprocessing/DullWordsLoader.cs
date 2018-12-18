using System.Collections.Generic;
using ResultOf;
using System.IO;

namespace TagsCloudVisualization.Preprocessing
{
    public class DullWordsLoader
    {
        private readonly string fileName;

        public DullWordsLoader(string fileName)
        {
            this.fileName = fileName;
        }

        public Result<HashSet<string>> LoadDullWords()
        { 
            return Result.Of(() => ReadWordsFromFile(fileName));
        }

        private HashSet<string> ReadWordsFromFile(string pathToFile)
        {
            var dullWords = new HashSet<string>();

            using (var reader = new StreamReader(pathToFile))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        break;
                    dullWords.Add(line.Trim().ToLower());
                }
            }

            return dullWords;
        }
    }
}
