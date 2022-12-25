using Xceed.Words.NET;

namespace TagsCloudVisualization.TextProviders;

public class DocTextProvider : TextFromFileProvider
{
    public DocTextProvider(string path) : base(path)
    {
    }

    protected override IEnumerable<string> GetFromSource(string path)
    {
        yield return DocX.Load(path).Text;
    }
}