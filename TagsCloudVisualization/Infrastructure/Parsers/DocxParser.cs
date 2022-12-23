using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spire.Doc;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.Infrastructure.Parsers
{
    public class DocxParser : IParser
    {
        private readonly DocumentParserHelper helper;
        private readonly ParserSettings settings;

        public DocxParser(ParserSettings settings)
        {
            helper = new DocumentParserHelper();
            this.settings = settings;
        }

        public string FileType => "docx";

        public Result<IEnumerable<string>> WordParse(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<IEnumerable<string>>($"файла по пути {path} не существует");

            var document = new Document(path, FileFormat.Docx);

            return Result.Ok(settings.TextType == TextType.OneWordOneLine
                ? helper.GetTextParagraph(document).Select(s => s.Trim())
                : helper.GetAllWordInDocument(document));
        }
    }
}