using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Parsers
{
    public class TxtParser : IParser
    {
        public string[] GetFormats()
        {
            return new[] { "txt" };
        }

        public Result<IEnumerable<string>> Parse(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<IEnumerable<string>>("Passed file doesn't exist!");
            return Result.Ok(ParseFile(path));
        }

        private IEnumerable<string> ParseFile(string path)
        {
            using var reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line is not null)
                    yield return line;
            }
        }
    }
}
