using ResultExtensions;
using ResultOf;
using System.Diagnostics;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Defaults.MyStem;

public class MyStem : ISingletonService, IDisposable
{
    private bool disposedValue = false;
    private static readonly ProcessStartInfo startInfo = new()
    {
        UseShellExecute = false,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        FileName = "mystem.exe",
        Arguments = "-nil -e cp866 - -"
    };
    private readonly Process myStemProccess;
    private bool isStarted = false;
    private readonly Dictionary<string, WordStat> cache = new();

    public MyStem()
    {
        myStemProccess = new()
        {
            StartInfo = startInfo
        };
    }

    public Result<WordStat> AnalyzeWord(string word)
    {
        if (disposedValue)
            throw new InvalidOperationException("MyStem object was disposed");

        if (word == null)
            return Result.Fail<WordStat>("Word was null");

        if (cache.TryGetValue(word, out var stat))
            return stat;

        if (!isStarted)
        {
            myStemProccess.Start();
            myStemProccess.BeginErrorReadLine();
            myStemProccess.ErrorDataReceived += HandleErrorData;
            isStarted = true;
        }

        myStemProccess.StandardInput.WriteLine(word);

        var stats = ParseWordStats(ReadAllLines(myStemProccess.StandardOutput));
        var statResult = stats.Length > 0 ? stats[0] : Result.Fail<WordStat>("MyStem could not understand a word");
        if (statResult.IsSuccess)
        {
            stat = statResult.GetValueOrThrow();
            cache[word] = stat;
            cache[stat.Stem] = stat;
        }

        return statResult;
    }

    private static IEnumerable<string> ReadAllLines(StreamReader reader)
    {
        yield return reader.ReadLine()!;
        while (reader.Peek() != -1)
        {
            yield return reader.ReadLine()!;
        }
    }

    private static Result<WordStat>[] ParseWordStats(IEnumerable<string> lines)
    {
        return lines.Select(ParseWordStat)
            .Where(x => x.IsSuccess)
            .OrderByDescending(x => x.GetValueOrThrow().Stem.Length)
            .ToArray();
    }

    private static Result<WordStat> ParseWordStat(string wordStat)
    {
        if (wordStat.EndsWith("??"))
            return Result.Fail<WordStat>("MyStem could not understand a word");
        var stemGrSeparation = wordStat.IndexOf('=');
        var grEnd = wordStat.IndexOf('=', stemGrSeparation + 1);
        var part = wordStat[(stemGrSeparation + 1)..grEnd].Split(',')[0];
        var stem = wordStat[..stemGrSeparation].TrimEnd('?');
        return new WordStat(stem, Enum.Parse<SpeechPart>(part));
    }

    private void HandleErrorData(object sender, DataReceivedEventArgs e)
    {
        throw new InvalidOperationException($"mystem.exe proccess produced an error: {e.Data}");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            myStemProccess.Dispose();
            disposedValue = true;
        }
    }

    ~MyStem()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
