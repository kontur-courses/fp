using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public class MyStem : IWordNormalizer
    {
        private readonly string workingDirectory;
        private readonly Regex pattern;

        public MyStem()
        {
            workingDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName,
                "MyStem");
            pattern = new Regex(@"{([a-zA-Zа-яА-Я]+)(\?.*)?}");
        }

        public Result<IEnumerable<string>> NormalizeWords(IEnumerable<string> words)
        {
            var stemInputFile = $"{Guid.NewGuid()}in.txt";
            var stemInputFilePath = Path.Combine(workingDirectory, stemInputFile);
            var stemOutputFile = $"{Guid.NewGuid()}out.txt";
            var stemOutputFilePath = Path.Combine(workingDirectory, stemOutputFile);
            return Result.OfAction(() => File.WriteAllLines(stemInputFilePath, words))
                .Then(none =>
                {
                    var arguments = $"/C mystem.exe {stemInputFile} {stemOutputFile} -cls";
                    var startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        Arguments = arguments,
                        WorkingDirectory = workingDirectory,
                        UseShellExecute = true
                    };
                    var process = new Process {StartInfo = startInfo};
                    process.Start();
                    process.WaitForExit();
                })
                .Then(none => File.ReadAllLines(stemOutputFilePath)
                    .Select(Parse)
                    .Select(s => s.ToLower()))
                .Then(lines =>
                {
                    File.Delete(stemInputFilePath);
                    File.Delete(stemOutputFilePath);
                    return lines;
                })
                .RefineError("MyStem error");
        }

        private string Parse(string line)
        {
            var match = pattern.Match(line).Groups;
            return match[1].Value;
        }
    }
}