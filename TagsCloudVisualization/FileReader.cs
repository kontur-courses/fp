#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Interfaces;

#endregion

namespace TagsCloudVisualization
{
    public class FileReader : IFileReader
    {
        public Result<IEnumerable<string>> GetWordsFromFile(string path, char[] separators)
        {
            if (!File.Exists(path)) return new Result<IEnumerable<string>>($"file {path} does not exist");

            var reader = new StreamReader(path);
            return new Result<IEnumerable<string>>(null, ReadAllWordsFromReader(reader, separators));
        }

        private static IEnumerable<string> ReadAllWordsFromReader(TextReader reader, char[] separators)
        {
            var line = reader.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                foreach (var word in line.Split(separators).Where(word => word.Length > 0))
                    yield return word;

                line = reader.ReadLine();
            }

            reader.Close();
        }
    }
}