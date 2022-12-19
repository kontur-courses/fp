using TagsCloud.Interfaces;

namespace TagsCloud
{
    public class TagCloud : ITagCloud
    {
        private readonly IBitmapper bitmapper;
        private readonly IMorphsFilter morphsFilter;
        private readonly IWordsFrequencyAnalyzer freqAnalyzer;
        private readonly IWordsRectanglesScaler wordsRectanglesScaler;
        private readonly IMorphsParser morphsParser;
        private readonly INormalFormParser normalFormParser;
        private readonly IFileValidator fileValidator;

        public TagCloud(IBitmapper bitmapper,
            IMorphsFilter morphsFilter,
            IWordsFrequencyAnalyzer freqAnalyzer,
            IWordsRectanglesScaler wordsRectanglesScaler,
            IMorphsParser morphsParser,
            INormalFormParser normalFormParser,
            IFileValidator fileValidator)
        {
            this.bitmapper = bitmapper;
            this.morphsFilter = morphsFilter;
            this.freqAnalyzer = freqAnalyzer;
            this.wordsRectanglesScaler = wordsRectanglesScaler;
            this.morphsParser = morphsParser;
            this.normalFormParser = normalFormParser;
            this.fileValidator = fileValidator;
        }

        public void PrintTagCloud(string textFilePath, string exportFilePath, string extension)
        {
            var validResult = fileValidator.VerifyFileExistence(textFilePath);

            var fullPath = exportFilePath + extension;

            var morphInfo = morphsParser.GetMorphs(validResult.GetValueOrThrow());

            var clearMorphs = morphsFilter.FilterRedundantWords(morphInfo);

            var normalFormWords = normalFormParser.Normalize(clearMorphs);

            var wordsFrequency = freqAnalyzer.GetSortedDictOfWordsFreq(normalFormWords);

            var relativeWordsSize = wordsRectanglesScaler.ConvertFreqToProportions(wordsFrequency);

            bitmapper.DrawWords(relativeWordsSize);
            bitmapper.SaveFile(fullPath);
        }
    }
}