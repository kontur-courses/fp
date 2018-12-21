using System.Linq;
using System.Text.RegularExpressions;
using TagCloud.Interfaces;

namespace TagCloud
{
    public class WordFilter : IWordFilter
    {
        private readonly bool ignoreBoring;
        private readonly string[] ignoringPartsOfSpeech;
        private readonly Regex parsePartOfSpeechRegex;
        private readonly string[] stopWords;

        public WordFilter(
            IFileReader fileReader,
            string path,
            bool ignoreBoring)
        {
            stopWords = path == "" ? new string[0] : fileReader.Read(path).GetValueOrThrow().ToArray();
            this.ignoreBoring = ignoreBoring;
            parsePartOfSpeechRegex = new Regex(@"\w*?=(\w+)");
            ignoringPartsOfSpeech = new[]
            {
                "PR",
                "PART",
                "CONJ",
                "SPRO",
                "ADVPRO",
                "APRO"
            };
        }

        public Result<bool> ToExclude(string word)
        {
            return IsBoring(word).Then(s => stopWords.Contains(word));
        }

        private Result<bool> IsBoring(string word)
        {
            return ignoreBoring
                ? ProgramExecuter.RunProgram(@"mystem.exe", "-l -i", word)
                    .Then(o => parsePartOfSpeechRegex.Match(o).Groups[1].Value)
                    .Then(partOfSpeech => ignoringPartsOfSpeech.Contains(partOfSpeech))
                : false;
        }
    }
}