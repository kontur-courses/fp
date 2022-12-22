using System.Diagnostics;
using System.Text;
using TagsCloudContainer.Interfaces;
using Result;

namespace TagsCloudContainer;

public class FileToDictionaryConverter : IConverter
{
    private readonly IWordsFilter filter;
    private readonly IDocParser parser;

    public FileToDictionaryConverter(IWordsFilter filter, IDocParser parser)
    {
        this.filter = filter;
        this.parser = parser;
    }

    public Result<Dictionary<string, int>> GetWordsInFile(ICustomOptions options)
    {
        var inputWordPath = Path.Combine(options.WorkingDir, options.WordsFileName);
        Result<List<string>> bufferedWordsResult;

        if (options.WordsFileName[options.WordsFileName.LastIndexOf('.')..] != ".txt")
            bufferedWordsResult = parser.ParseDoc(inputWordPath);
        else
        {
            var wordsInFile = File.ReadAllLines(inputWordPath)
                .ToList();
            bufferedWordsResult = wordsInFile.Count != 0
                ? new Result<List<string>>(wordsInFile)
                : new Result<List<string>>(new Exception("Words file are empty"));
        }

        if (!bufferedWordsResult)
            return new Result<Dictionary<string, int>>(bufferedWordsResult.Exception!);
        var bufferedWords = bufferedWordsResult.Value
            .Select(x => x.ToLower())
            .ToList();
        var tmpFilePath = Path.Combine(options.WorkingDir, "tmp.txt");
        File.WriteAllLines(tmpFilePath, bufferedWords);

        var cmd = $"mystem.exe -nig {tmpFilePath}";

        var processStartInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            WorkingDirectory = Path.Combine(options.WorkingDir),
            FileName = @"C:\Windows\System32\cmd.exe",
            Arguments = "/C" + cmd,
            RedirectStandardOutput = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            StandardOutputEncoding = Encoding.UTF8,
        };
        var process = Process.Start(processStartInfo);

        var taggedWords = process.StandardOutput
            .ReadToEnd()
            .Split("\r\n")
            .ToList();
        process.Close();

        File.Delete(tmpFilePath);

        var boringWords = File.ReadAllLines(Path.Combine(options.WorkingDir, options.BoringWordsName))
            .Select(x => x.ToLower())
            .ToList();

        boringWords.Sort();

        var boringWordsSet = boringWords.ToHashSet();

        var filteredWords = filter.FilterWords(taggedWords, options, boringWordsSet);

        var result = new Dictionary<string, int>();
        filteredWords.Value.ForEach(x =>
        {
            if (result.ContainsKey(x))
                result[x] += 1;

            else result.Add(x, 1);
        });

        return new Result<Dictionary<string, int>>(result
            .ToList()
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value));
    }
}