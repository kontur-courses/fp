using System.Collections.Immutable;

namespace TagsCloud.FileParsers
{
    public class DocParser //: IFileParser
    {
        public string[] FileExtensions => new string[] { ".doc", ".docx" };

        public Result<ImmutableList<string>> Parse(string filename)
        {
            return Result.Of(() => ImmutableList.Create("Съешь ещё этих мягких французских булок да выпей же чаю".Split(' ')));
        }
    }
}
