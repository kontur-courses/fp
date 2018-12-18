using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using TagCloud.Result;

namespace TagCloud
{
    public static class ProgramExecuter
    {
        public static Result<string> RunProgram(string command, string paramsString, string content)
        {
            using (var process = CreateProcess(command, paramsString))
            {
                process.Start();
                WriteToStream(process, content);
                return ReadFromStream(process);
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
                return Result.Result.OfAction(() => writer.WriteLine(content));
            }
        }

        public static Result<string> ReadFromStream(Process process)
        {
            using (var reader = new StreamReader(process.StandardOutput.BaseStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}