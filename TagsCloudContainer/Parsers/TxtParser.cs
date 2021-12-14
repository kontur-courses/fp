using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer 
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
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
            }
        }
    }
}
