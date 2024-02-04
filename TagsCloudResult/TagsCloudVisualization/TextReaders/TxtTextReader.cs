using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.TextReaders;

public class TxtTextReader : TextReader
{
    public TxtTextReader(SourceSettings settings) : base(settings)
    {
    }

    public override Result<string> GetText()
    {
        using var reader = new StreamReader(Settings.Path);
        return reader.ReadToEnd();
    }
}