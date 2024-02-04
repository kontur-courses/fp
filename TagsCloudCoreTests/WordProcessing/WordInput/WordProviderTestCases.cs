using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCoreTests.WordProcessing.WordInput;

public class WordProviderTestCases
{
    public static IEnumerable<IWordProvider> Providers
    {
        get
        {
            yield return new TxtFileWordParser();
            yield return new DocxFileWordParser();
        }
    }
}