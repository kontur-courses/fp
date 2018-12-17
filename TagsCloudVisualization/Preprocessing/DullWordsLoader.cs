using System.Collections.Generic;
using ResultOf;
using System.IO;
using System.Reflection;

namespace TagsCloudVisualization.Preprocessing
{
    public class DullWordsLoader
    {
        public static Result<HashSet<string>> LoadDullWords(string fileName)
        {
            var pathToAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var pathToDullWords = Path.Combine(
                pathToAssemblyDirectory, "Resources", "dull_words.txt"
            );

            return Result.Of(() => ReadWordsFromFile(pathToDullWords));
        }

        private static HashSet<string> ReadWordsFromFile(string pathToFile)
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
