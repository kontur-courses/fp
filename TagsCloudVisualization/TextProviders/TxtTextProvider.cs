namespace TagsCloudVisualization.TextProviders;

public class TxtTextProvider : TextFromFileProvider
{
    public TxtTextProvider(string path) : base(path)
    {
    }
    
    protected override IEnumerable<string> GetFromSource(string path)
    {
        return File.ReadLines(path);
    }
}