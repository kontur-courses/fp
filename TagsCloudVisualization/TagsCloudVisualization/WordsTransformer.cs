using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NHunspell;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class WordsTransformer : IWordsTransformer
    {
        public Result<List<string>> GetStems(List<string> words)
        {
            return GetPath()
                .Then(path => IsHunspellDictExists($"{path}en_us.dic")
                    .Then(t => IsHunspellDictExists($"{path}en_us.aff"))
                    .Then(t => GetStemsHunSpell(words, path))
                    .OnFail(Console.WriteLine));
        }

        private static Result<string> GetPath()
        {
            var path = Application.StartupPath;
            return $"{path.Substring(0, path.IndexOf("bin", StringComparison.Ordinal))}NHunSpell\\";
        }

        private Result<List<string>> GetStemsHunSpell(IEnumerable<string> words, string path)
        {
            var stems = new List<string>();

            using (var hunspell = new Hunspell($"{path}en_us.aff", $"{path}en_us.dic"))
            {
                foreach (var word in words)
                    stems.Add(hunspell.Stem(word).FirstOrDefault() ?? word);
            }

            return stems;
        }

        private Result<string> IsHunspellDictExists(string dic)
        {
            return Result.Validate(dic, File.Exists, $"Hunspell dictionary not found in '{dic}'");
        }
    }
}