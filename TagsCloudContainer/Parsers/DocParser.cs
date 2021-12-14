using TagsCloudContainer.Interfaces;
using Application = Microsoft.Office.Interop.Word.Application;

namespace TagsCloudContainer 
{
    public class DocParser : IParser
    {
        private readonly char[] separators = { '\r', '\n' };

        public string[] GetFormats()
        {
            return new[] { "doc", "docx" };
        }

        public Result<IEnumerable<string>> Parse(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<IEnumerable<string>>("Passed file doesn't exist!");
            return Result.Ok(ParseFile(path));
        }

        private IEnumerable<string> ParseFile(string path)
        {
            var application = new Application { Visible = false };
            var document = application.Documents.Open(path);

            foreach (var word in document.Range().Text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries))
                yield return word;

            document.Close();
            application.Quit();
        }
    }
}
