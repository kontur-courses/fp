﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Text;
using TagCloud.ResultMonade;

namespace TagCloud.FileReader
{
    public class DocxFileReader : IFileReader
    {
        public Result<string> ReadAllText(string filePath)
        {
            if (!filePath.EndsWith(".doc") && !filePath.EndsWith(".docx"))
                return Result.Fail<string>($"File {filePath} isn't format .doc or .docx");

            if (!File.Exists(filePath))
                return Result.Fail<string>($"File {filePath} doesn't exist");

            var sb = new StringBuilder();

            using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Open(filePath, false))
            {
                var paragraphs = wordDocument.MainDocumentPart.RootElement.Descendants<Paragraph>();

                foreach (var paragraph in paragraphs)
                    sb.AppendLine(paragraph.InnerText);

                return sb.ToString().Trim();
            }
        }
    }
}