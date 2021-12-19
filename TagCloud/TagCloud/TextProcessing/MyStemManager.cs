using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ResultOf;

namespace TagCloud.TextProcessing
{
    internal class MyStemManager : IMorphologyAnalyzer
    {
        private const string UtilFileName = "mystem.exe";
        private const string TempPath = @"c:\temp\output.txt";
        private const string Arguments = "-nl -ig -d --format json";

        public Result<IEnumerable<ILexeme>> GetLexemesFrom(string filePath)
        {
            var myStemResults = RunMyStem(filePath)
                .Then(_ => Result.Of(ParseMyStemResult));
            if (myStemResults.IsSuccess)
                File.Delete(TempPath);
            return myStemResults;
        }

        private static Result<None> RunMyStem(string filePath)
        {
            using var process = ConfigureProcess(filePath);
            var myStemErrorOut = string.Empty;
            process.ErrorDataReceived += (s, e) => myStemErrorOut += e.Data;

            return Result
                .Of(() => process.Start(),
                    "MyStem не запустился, проверьте наличие утилиты в корневом каталоге программы")
                .Then(_ => process.BeginErrorReadLine())
                .Then(_ => process.WaitForExit())
                .Then(_ => !string.IsNullOrEmpty(myStemErrorOut)
                    ? Result.Fail<None>(
                        "MyStem отработал с ошибками, обработка текста прервана.\n" +
                        "Проверьте исходный текст на соответствие кодировке UTF-8.\n" +
                        $"Вывод MyStem: {myStemErrorOut}")
                    : Result.Ok());
        }

        private static IEnumerable<ILexeme?> ParseMyStemResult()
        {
            return File.ReadAllText(TempPath)
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(JsonConvert.DeserializeObject<MyStemResultDto>)
                .Select(MyStemResult.FromDto)
                .Where(r => r.Analysis.Count > 0);
        }

        private static Process ConfigureProcess(string filepath)
        {
            var process = new Process();
            process.StartInfo.FileName = UtilFileName;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.Arguments = $"{Arguments} {filepath} {TempPath}";
            return process;
        }
    }
}