using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Functional;
using Xceed.Words.NET;

namespace TagsCloudContainer.Data.Readers
{
    public class DocWordsFileReader : IFileFormatReader
    {
        public IEnumerable<string> Extensions { get; } = new[] {".doc", ".docx"};

        public Result<IEnumerable<string>> ReadAllWords(string path)
        {
            return Result.Of(() => DocX.Load(path).Paragraphs.Select(p => p.Text));
        }
    }
}