using MystemHandler;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Lemmaizers;

public class Lemmaizer : ILemmaizer
{
    private readonly MystemMultiThread mt;
    public Mystem.Net.Mystem mystem;

    public Lemmaizer(string pathToMyStem)
    {
        mt = File.Exists(pathToMyStem)
            ? new MystemMultiThread(1, pathToMyStem)
            : null;
        mystem = new Mystem.Net.Mystem();
    }

    public Result<string> GetMorph(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return Result.Fail<string>("Word is white space or null");

        return mystem.Mystem.Analyze(word).Result[0].AnalysisResults[0].Grammeme.Split(',', '=')[0];
    }

    public Result<string> GetLemma(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return Result.Fail<string>("Word is white space or null");

        if (mt == null)
            return Result.Fail<string>("mystem.exe not found at given path");

        return mt.StemOneWord(word);
    }
}