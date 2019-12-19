using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloud.FileParsers
{
    public class DocParser //: IFileParser
    {
        public string[] FileExtensions => new string[] { ".doc", ".docx" };

        public Result<ImmutableList<string>> Parse(string filename)
        {
            return Result.Of(() => ImmutableList.ToImmutableList(
                Regex.Split("Съешь ещё этих мягких французских булок, да выпей же чаю!", @"\W+")
                .Where(item => !string.IsNullOrEmpty(item))));
        }
    }
}
