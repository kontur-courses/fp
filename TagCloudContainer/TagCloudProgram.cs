using System.Collections.Generic;
using System.Linq;
using ResultOfTask;
using TagsCloudPreprocessor;
using TagsCloudPreprocessor.Preprocessors;
using TagsCloudVisualization;

namespace TagCloudContainer
{
    public class TagCloudProgram
    {
        private readonly Config config;
        private readonly IFileReader fileReader;
        private readonly ITextParser parser;
        private readonly ITagCloudVisualization tagCloudVisualization;
        private readonly IPreprocessor wordsExcluder;
        private readonly IPreprocessor wordsStemer;

        public TagCloudProgram(
            ITagCloudVisualization tagCloudVisualization,
            ITextParser parser,
            IFileReader fileReader,
            IPreprocessor wordsExcluder,
            IPreprocessor wordsStemer,
            Config config)
        {
            this.tagCloudVisualization = tagCloudVisualization;
            this.config = config;
            this.wordsExcluder = wordsExcluder;
            this.wordsStemer = wordsStemer;
            this.fileReader = fileReader;
            this.parser = parser;
        }

        public Result<None> SaveTagCloud()
        {
            var words = fileReader
                .ReadFromFile(config.InputFile)
                .Then(parser.GetWords)
                .Then(x => x.ToList())
                .Then(wordsExcluder.PreprocessWords)
                .Then(wordsStemer.PreprocessWords);


            return Result.Of(() => tagCloudVisualization.SaveTagCloud(
                    config.FileName,
                    config.OutPath,
                    words.Then(GetFrequencyDictionary)
                        .Then(x => x.OrderBy(p => p.Value))
                        .Then(x => x.Reverse())
                        .Then(x => x.Take(config.Count))
                        .Then(x => x.ToDictionary(p => p.Key, p => p.Value)))
                .GetValueOrThrow());
        }

        private Dictionary<string, int> GetFrequencyDictionary(List<string> words)
        {
            var frequencyDictionary = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!frequencyDictionary.ContainsKey(word))
                    frequencyDictionary[word] = 1;
                frequencyDictionary[word]++;
            }

            return frequencyDictionary;
        }
    }
}