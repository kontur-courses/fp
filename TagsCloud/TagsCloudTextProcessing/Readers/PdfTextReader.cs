using System.IO;
using BitMiracle.Docotic.Pdf;
using TagCloudResult;

namespace TagsCloudTextProcessing.Readers
{
    public class PdfTextReader : ITextReader
    {
        private readonly string path;

        public PdfTextReader(string path)
        {
            this.path = path;
        }

        public Result<string> ReadText()
        {
            if(!File.Exists(path))
                return Result.Fail<string>($"FILE {path} doesn't exist");
            
            using (var pdfDocument = new PdfDocument(path))
                return Result.Ok(pdfDocument.GetText());
        }
    }
}