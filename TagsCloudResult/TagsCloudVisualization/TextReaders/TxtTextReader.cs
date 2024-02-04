using TagsCloudVisualization.Common;

namespace TagsCloudVisualization.TextReaders;

public class TxtTextReader : TextReader
{
    public TxtTextReader(SourceSettings settings) : base(settings)
    {
    }

    protected override Result<string> ReadText(string path)
    {
        using var reader = new StreamReader(path);
        return reader.ReadToEnd();
    }
}