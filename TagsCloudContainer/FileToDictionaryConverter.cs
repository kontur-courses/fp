using System.Diagnostics;
using System.Text;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

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
        var inputWordPath = Path.Combine(options.WorkingDirectory, options.WordsFileName);
        Result<List<string>> bufferedWordsResult;

        if (options.WordsFileName[options.WordsFileName.LastIndexOf('.')..] != ".txt")
            bufferedWordsResult = parser.ParseDoc(inputWordPath);
        else
        {
            var wordsInFile = File.ReadAllLines(inputWordPath)
                .ToList();
            bufferedWordsResult = wordsInFile.Count != 0
                ? wordsInFile.AsResult()
                : Result.Fail<List<string>>("Words file are empty");
        }

        if (!bufferedWordsResult.IsSuccess)
            return Result.Fail<Dictionary<string, int>>(bufferedWordsResult.Error);
        var bufferedWords = bufferedWordsResult.GetValueOrThrow()
            .Select(x => x.ToLower())
            .ToList();
        var tmpFilePath = Path.Combine(options.WorkingDirectory, "tmp.txt");
        File.WriteAllLines(tmpFilePath, bufferedWords);

        var cmd = $"mystem.exe -nig {tmpFilePath}";

        var processStartInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            WorkingDirectory = Path.Combine(options.WorkingDirectory),
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

        var boringWords = File.ReadAllLines(Path.Combine(options.WorkingDirectory, options.BoringWordsName))
            .Select(x => x.ToLower())
            .ToList();

        boringWords.Sort();

        var boringWordsSet = boringWords.ToHashSet();

        var filteredWords = filter.FilterWords(taggedWords, options, boringWordsSet);

        var result = new Dictionary<string, int>();
        filteredWords.GetValueOrThrow().ForEach(x =>
        {
            if (result.ContainsKey(x))
                result[x] += 1;

            else result.Add(x, 1);
        });

        return result
            .ToList()
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value)
            .AsResult();
    }
}