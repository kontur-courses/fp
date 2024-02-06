using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.TextReaders;

public abstract class TextReader
{
    protected readonly SourceSettings Settings;

    protected TextReader(SourceSettings settings)
    {
        Settings = settings;
    }

    public abstract Result<string> GetText();
}
