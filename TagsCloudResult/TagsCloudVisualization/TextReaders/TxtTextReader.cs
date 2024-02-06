using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public class TxtTextReader : ITextReader
{
    private readonly SourceSettings settings;
    
    public TxtTextReader(SourceSettings settings)
    {
        this.settings = settings;
    }

    public  Result<string> GetText()
    {
        using var reader = new StreamReader(settings.Path);
        return reader.ReadToEnd();
    }
}
