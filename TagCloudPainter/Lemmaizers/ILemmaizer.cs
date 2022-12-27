using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Lemmaizers;

public interface ILemmaizer
{
    public Result<string> GetMorph(string word);
    public Result<string> GetLemma(string word);
}