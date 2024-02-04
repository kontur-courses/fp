using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace TagsCloudVisualization;

public class MystemWordsParser : IInterestingWordsParser
{
    private IDullWordChecker dullWordChecker;

    public MystemWordsParser(IDullWordChecker dullWordChecker)
    {
        this.dullWordChecker = dullWordChecker;
    }

    public Result<IEnumerable<string>> GetInterestingWords(string inputFilePath)
    {
        var path = Path.GetFullPath(inputFilePath);

        return Result.Of(() => File.ReadAllText(path, Encoding.UTF8))
            .RefineErrorOnCondition(!Path.IsPathRooted(inputFilePath),
                "Relative paths are searched realative to .exe file. Try giving an absolute path")
            .Then(ParseInterestingWords)
            .Then(analyses => analyses
                .Where(analysis => !dullWordChecker.Check(analysis))
                .Select(analysis => analysis.Lexema.ToLower()))
            .RefineError("Can't get interesting words");
    }

    private static Result<List<WordAnalysis>> ParseInterestingWords(string text)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "mystem.exe"),
                Arguments = "-lig --format json",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardInputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
            }
        };
        process.Start();
        process.StandardInput.Write(text);
        process.StandardInput.Close();

        var wordsAnalysis = Result.Of(() => DeserializeInterestingWordsAnalysis(process))
            .RefineError("Error occured during deserialization");

        process.WaitForExit(1);
        return wordsAnalysis;
    }

    private static List<WordAnalysis> DeserializeInterestingWordsAnalysis(Process process)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var wordsAnalysis = new List<WordAnalysis>();

        while (!process.StandardOutput.EndOfStream)
        {
            var line = process.StandardOutput.ReadLine();
            var deserializedLine = JsonSerializer.Deserialize<List<JsonWordAnalysis>>(line, options);
            foreach (var jsonWordAnalysis in deserializedLine)
            {
                if (jsonWordAnalysis.Analysis.Count < 1)
                    continue;

                var unpackedAnalysis = jsonWordAnalysis.Analysis.First();
                wordsAnalysis.Add(new WordAnalysis(jsonWordAnalysis.Text, unpackedAnalysis["lex"],
                    unpackedAnalysis["gr"]));
            }
        }
        
        return wordsAnalysis;
    }

    private class JsonWordAnalysis
    {
        public string Text { get; set; }
        public List<Dictionary<string, string>> Analysis { get; set; }
    }
}