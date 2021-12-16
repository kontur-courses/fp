using System.Collections.Generic;
using System.IO;
using TagsCloudContainer;

namespace TagsCloudApp.WordsLoading
{
    public class TxtFileTextLoader : TextFileLoader
    {
        public override IEnumerable<FileType> SupportedTypes => new[] {FileType.Txt};

        protected override Result<string> LoadTextFromExistingFile(string filename) =>
            File.ReadAllText(filename);
    }
}