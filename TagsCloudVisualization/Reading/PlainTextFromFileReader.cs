namespace TagsCloudVisualization.Reading;

public class PlainTextFromFileReader : ITextReader
{
    private readonly string _pathToFile;

    public PlainTextFromFileReader(string pathToFile)
    {
        _pathToFile = pathToFile;
    }

    public Result<string> ReadText()
    {
        if (!File.Exists(_pathToFile))
            return Result.Fail<string>($"File not exists {_pathToFile}");

        try
        {
            var text = File.ReadAllText(_pathToFile);
            return text;
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.ToString());
        }
    }
}