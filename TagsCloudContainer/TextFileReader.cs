using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudContainer
{
    public class TextFileReader : ISource
    {
        private readonly string filename;

        public TextFileReader(string filename)
        {
            this.filename = filename;
        }

        public Result<string[]> GetWords()
        {
            if (filename == null)
                return Result.Ok(new string[0]);

            return Result.Of(ReadWords);
        }

        private string[] ReadWords()
        {
            var text = File.ReadAllText(filename);
            var matches = Regex.Matches(text, @"\b[\w']+\b");

            var words = from m in matches.Cast<Match>()
                select m.Value;

            return words.ToArray();
        }
    }
}
