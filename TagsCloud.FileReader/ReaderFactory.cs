using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public class ReaderFactory : IReaderFactory
    {
        public Result<IWordsReader> GetReader(string extension)
        {
            switch (extension)
            {
                case "txt":
                    return new TxtReader();
                case "rtf":
                    return new RtfReader();
                case "docx":
                    return new DocxReader();
                default:
                    return Result.Fail<IWordsReader>("not valid extension of file");
            }
        }
    }
}