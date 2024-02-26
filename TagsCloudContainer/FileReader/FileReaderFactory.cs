using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.FileReader;

public class FileReaderFactory
{
    public Result<ITextReader> GetReader(string filePath)
    {
        var fileExtension = Path.GetExtension(filePath);
        return fileExtension switch
        {
            ".docx" => new DocxFileReader().Ok<ITextReader>(),
            ".txt" => new TxtFileReader().Ok<ITextReader>(),
            _ => Result.Fail<ITextReader>($"Неверный формат файла: {fileExtension}")
        };
    }
}
