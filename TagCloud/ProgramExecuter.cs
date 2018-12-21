using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;

namespace TagCloud
{
    public static class ProgramExecuter
    {
        public static Result<string> RunProgram(string command, string paramsString, string content)
        {
            using (var process = CreateProcess(command, paramsString))
            {
                return Result.Of(() => process.Start())
                    .Then(_ => WriteToStream(process, content))
                    .Then(_ => ReadFromStream(process));
            }
        }

        public static Process CreateProcess(string command, string paramsString)
        {
            return new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = paramsString,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                }
            };
        }

        public static Result<None> WriteToStream(Process process, string content)
        {
            using (var writer = new StreamWriter(process.StandardInput.BaseStream))
            {
                return Result.OfAction(() => writer.WriteLine(content));
            }
        }

        public static Result<string> ReadFromStream(Process process)
        {
            using (var reader = new StreamReader(process.StandardOutput.BaseStream))
            {
                return Result.Of(() => reader.ReadToEnd());
            }
        }
    }
}