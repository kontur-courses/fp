using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TagsCloudContainer.FileReaders
{
    public class DocFileReader : IFileReader
    {
        public HashSet<string> SupportedFormats { get; } = new HashSet<string>() { ".doc", ".docx" };

        public Result<IEnumerable<string>> ReadWordsFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return Result.Fail<IEnumerable<string>>($"File doesn't exist {path}");
            }

            using var doc = WordprocessingDocument.Open(path, false);
            var words = doc.MainDocumentPart.Document.Body.Descendants<Paragraph>()
                .Select(x => x.InnerText);

            return Result.Ok(words);
        }
    }
}