using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TagsCloudGenerator.Core.Filters
{
    public class WordsFilter : IWordsFilter
    {
        private readonly string[] unusedPartsOfSpeech = {"PR", "ADV", "CONJ", "PART", "SPRO"};
        private readonly string pathToMyStem32;
        private readonly string pathToMyStem64;

        public WordsFilter(string pathToMyStem32, string pathToMyStem64)
        {
            this.pathToMyStem32 = pathToMyStem32;
            this.pathToMyStem64 = pathToMyStem64;
        }

        public IEnumerable<string> GetFilteredWords(IEnumerable<string> words)
        {
            var myStemOutput = GetMyStemOutput(words);

            var filteredWords = myStemOutput
                .Split('\n')
                .Where(s => s != "" && !unusedPartsOfSpeech.Any(p => s.Contains($"={p}")))
                .Select(s => s.Split('{')[0]);
            foreach (var word in filteredWords)
                yield return word;
        }

        private string GetMyStemOutput(IEnumerable<string> words)
        {
            File.WriteAllLines("temp.txt", words);          
            
            var result = "";

            using (var myStemProcess = new Process())
            {
                myStemProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                myStemProcess.StartInfo.FileName = Environment.Is64BitProcess
                    ? pathToMyStem64
                    : pathToMyStem32;
                myStemProcess.StartInfo.Arguments = "-ni temp.txt";
                myStemProcess.StartInfo.CreateNoWindow = true;
                myStemProcess.StartInfo.UseShellExecute = false;
                myStemProcess.StartInfo.RedirectStandardInput = true;
                myStemProcess.StartInfo.RedirectStandardOutput = true;
                myStemProcess.Start();
                result += myStemProcess.StandardOutput.ReadToEnd();
            }

            File.Delete("temp.txt");

            return result;
        }
    }
}