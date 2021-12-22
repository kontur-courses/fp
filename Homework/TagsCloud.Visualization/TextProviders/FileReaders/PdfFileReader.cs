using System;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TextProviders.FileReaders
{
    public class PdfFileReader : IFileReader
    {
        public Result<string> Read(string filename)
        {
            try
            {
                var text = new StringBuilder();

                using var reader = new PdfReader(filename);

                for (var i = 1; i <= reader.NumberOfPages; i++)
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));

                return text.ToString();
            }
            catch (Exception e)
            {
                return Result.Fail<string>(e.Message);
            }
        }

        public bool CanRead(string extension) => extension == "pdf";
    }
}