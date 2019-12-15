using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.MyStem
{
    class MyStemProcess
    {
        private readonly Process myStemProcess;

        private readonly string inputFileName;
        private readonly string outputFileName;

        public MyStemProcess()
        {
            inputFileName = Guid.NewGuid() + ".txt";
            outputFileName = Guid.NewGuid() + ".txt";

            myStemProcess = new Process
            {
                StartInfo =
                {
                    FileName = "exe/mystem.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    Arguments = $"-ni {inputFileName} {outputFileName}"
                }
            };
        }

        private Result<IEnumerable<Word>> RunMystemOn(Stream stream)
        {
            try
            {
                using (var fs = File.Create(inputFileName))
                {
                    stream.CopyTo(fs);
                }

                myStemProcess.Start();
                myStemProcess.WaitForExit();
                myStemProcess.Close();

                File.Delete(inputFileName);

                var words = new List<Word>();

                using (var streamReader = File.OpenText(outputFileName))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var (isSuccess, _, value) = Word.FromMyStemConclusion(line);
                        if (isSuccess)
                            words.Add(value);
                    }
                }

                File.Delete(outputFileName);

                return Result.Ok<IEnumerable<Word>>(words);
            }
            catch (Exception e)
            {
                return Result.Failure<IEnumerable<Word>>("Mystem process failed: " + e.Message);
            }
        }

        public Result<IEnumerable<string>> StreamToWords(Stream stream)
        {
            return 
                RunMystemOn(stream)
                .Map(words => words.Select(word => word.Grammar.InitialForm));
        }

        private static Stream StreamFromString(string s)
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
            }
            return stream;
        }

        public Result<IEnumerable<Word>> GetWordsWithGrammar(IEnumerable<string> words)
        {
            return RunMystemOn(StreamFromString(words.Aggregate("\n", (s1, s2) => s1 + "\n" + s2)));
        }
    }
}