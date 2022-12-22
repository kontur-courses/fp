namespace TagsCloudContainer.TextReaders
{
    public class TextReaderGenerator
    {
        public Result<ITextReader> GetReader(string pathToFile)
        {
            var extension = Path.GetExtension(pathToFile);
            return extension switch
            {
                ".txt" => new TxtReader().Ok<ITextReader>(),
                ".docx" => new WordReader().Ok<ITextReader>(),
                _ => Result.Fail<ITextReader>($"This file format is not supported: {extension}")
            };
        }
    }
}