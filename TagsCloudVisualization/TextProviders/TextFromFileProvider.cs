namespace TagsCloudVisualization.TextProviders;

public abstract class TextFromFileProvider : ITextProvider
{
    private readonly string path;

    protected TextFromFileProvider(string path)
    {
        this.path = path;
    }
    public Result<IEnumerable<string>> GetText()
    {
        return path.AsResult()
            .Validate(File.Exists, $"Path {path} not found")
            .Then(GetFromSource);
    }
    

    protected abstract IEnumerable<string> GetFromSource(string path);
}