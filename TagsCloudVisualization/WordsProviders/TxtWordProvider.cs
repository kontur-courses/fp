using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization.WordsProviders
{
    public class TxtWordProvider : IWordProvider
    {
        public Result<List<string>> GetWords(string filepath)
        {
            var words = new List<string>();

            if (!File.Exists(filepath))
                return Result.Fail<List<string>>("File with words was not found");

            using var sr = new StreamReader(filepath);
            string word;
            while ((word = sr.ReadLine()) != null) words.Add(word);

            return words;
        }
    }
}