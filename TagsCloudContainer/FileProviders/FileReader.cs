namespace TagsCloudContainer.FileProviders;

public class FileReader : IFileReader
{
    public Result<string> ReadFile(string filePath)
    {
        string text;
        try
        {
            text = File.ReadAllText(filePath);
        }
        catch (DirectoryNotFoundException e)
        {
            return Result.Fail<string>("Директория не найдена.");
        }
        catch (FileNotFoundException e)
        {
            return Result.Fail<string>("Файл не найден.");
        }
        catch (Exception e)
        {
            return Result.Fail<string>("Ошибка при чтении файла. " + e.Message);
        }

        return text;
    }
}