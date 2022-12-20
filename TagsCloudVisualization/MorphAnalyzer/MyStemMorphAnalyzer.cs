using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace TagsCloudVisualization.MorphAnalyzer;

public class MyStemMorphAnalyzer : IMorphAnalyzer
{
    private readonly string _workingDirectory;
    private const string TempFileName = "mystemTemp.txt";

    public MyStemMorphAnalyzer(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
    }

    public Result<Dictionary<string, WordMorphInfo>> GetWordsMorphInfo(IEnumerable<string> words)
    {
        if (!File.Exists(Path.Combine(_workingDirectory, "mystem.exe")))
            return Result.Fail<Dictionary<string, WordMorphInfo>>($"File mystem.exe not found");
        
        var pathToTempFileName = Path.Combine(_workingDirectory, TempFileName);

        try
        {
            File.WriteAllLines(pathToTempFileName, words);
        }
        catch (Exception e)
        {
            return Result.Fail<Dictionary<string, WordMorphInfo>>(e.ToString()).RefineError("Error write temp file for morph words");
        }

        var proc = new ProcessStartInfo
        {
            UseShellExecute = false,
            WorkingDirectory = Path.Combine(_workingDirectory),
            FileName = @"C:\Windows\System32\cmd.exe",
            Arguments = $"/C mystem.exe -nig --format json {TempFileName}",
            RedirectStandardOutput = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            StandardOutputEncoding = Encoding.UTF8,
        };
        var process = Process.Start(proc);


        var wordsFromMorphAnalyzer = process?.StandardOutput
            .ReadToEnd()
            .Split(Environment.NewLine)
            .ToList()!;

        if (File.Exists(pathToTempFileName))
        {
            try
            {
                File.Delete(pathToTempFileName);
            }
            catch (Exception e)
            {
                return Result.Fail<Dictionary<string, WordMorphInfo>>(e.ToString()).RefineError("Error delete temp file for morph words");
            }
        }

        return ParseWordMorphInfoFromOutput(wordsFromMorphAnalyzer);
    }

    private static Dictionary<string, WordMorphInfo> ParseWordMorphInfoFromOutput(List<string> wordsFromMorphAnalyzer)
    {
        var wordsSpeechInfo = new Dictionary<string, WordMorphInfo>();
        foreach (var line in wordsFromMorphAnalyzer)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            var wordInfo = JsonSerializer.Deserialize<MyStemWordInfo>(line);
            if (wordInfo == null)
                continue;

            var wordMorphInfo = new WordMorphInfo();
            foreach (var wordForm in wordInfo.Analysis)
            {
                wordMorphInfo.PartsOfSpeech.Add(wordForm.Gr.Split(',', '=')[0]);
            }

            wordsSpeechInfo[wordInfo.Text] = wordMorphInfo;
        }

        return wordsSpeechInfo;
    }
}