using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TagsCloud.WordsFiltering
{
    public class WasteWordsFilter : IFilter
    {
        public bool AllowAdjectives { get; set; } = true; //A
        public bool AllowAdverbs { get; set; } = false; //ADV
        public bool AllowPronounAdverb { get; set; } = false; //ADVPRO
        public bool AllowNumeralAdjectives { get; set; } = false; //ANUM
        public bool AllowPronounAdjectives { get; set; } = false; //APRO
        public bool AllowComposites { get; set; } = false; //COM
        public bool AllowUnions { get; set; } = false; //CONJ
        public bool AllowInterjections { get; set; } = false; //ITNJ
        public bool AllowNumerals { get; set; } = false; //NUM
        public bool AllowParticles { get; set; } = false; //PART
        public bool AllowPrepositions { get; set; } = false; //PR
        public bool AllowNouns { get; set; } = true; //S
        public bool AllowPronouns { get; set; } = false; //SPRO
        public bool AllowVerbs { get; set; } = true; //V

        private readonly Dictionary<string, bool> allowDict;

        public WasteWordsFilter()
        {
            allowDict = new Dictionary<string, bool>
            {
                { "A", AllowAdjectives },
                { "ADV", AllowAdverbs },
                { "ADVPRO", AllowPronounAdverb },
                { "ANUM", AllowNumeralAdjectives },
                { "APRO", AllowPronounAdjectives },
                { "COM", AllowComposites },
                { "CONJ", AllowUnions },
                { "ITNJ", AllowInterjections },
                { "NUM", AllowNumerals },
                { "PART", AllowParticles },
                { "PR", AllowPrepositions },
                { "S", AllowNouns },
                { "SPRO", AllowPronouns },
                { "V", AllowVerbs }
            };
        }

        public Result<ImmutableList<string>> FilterWords(ImmutableList<string> words)
        {
            return Task.Run(() => GetGrammems(words)).Result
                .Then(grammems => ImmutableList.ToImmutableList(grammems
                    .Select(grammem => grammem.Split(new char[] { '=', ',' }, 4))
                    .Where(grInfo => grInfo.Length == 2 || (grInfo.Length >= 3 && grInfo[2] != "сокр"))
                    .Where(grInfo => allowDict.TryGetValue(grInfo[1], out var isAllow) && isAllow)
                    .Select(grInfo => grInfo[0].Trim('?'))));
        }

        private Result<string[]> GetGrammems(IEnumerable<string> words)
        {
            return Result.Of(() =>
            {
                var input = Path.GetTempFileName();
                File.WriteAllLines(input, words);
                var output = Path.GetTempFileName();

                var asmLocation = Assembly.GetExecutingAssembly().Location;
                var path = Path.GetDirectoryName(asmLocation);
                using (var process = new Process())
                {
                    process.StartInfo.FileName = $"{path}\\WordsFiltering\\mystem.exe";
                    process.StartInfo.Arguments = $"-nldig \"{input}\" \"{output}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.EnableRaisingEvents = true;
                    process.Start();
                    if (!process.WaitForExit(30000))
                        throw new TimeoutException("'Mystem' operation timeout reached.");
                }

                return output;
            })
                .Then(output => Result.Of(() => File.ReadAllLines(output)).RefineError($"Can't read file '{output}'."))
                .RefineError("'Mystem' can't analyse words for grammems.");
        }
    }
}
