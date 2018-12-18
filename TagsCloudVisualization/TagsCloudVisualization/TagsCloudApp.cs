using System;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization
{
    public class TagsCloudApp
    {
        private readonly ICloudParametersParser cloudParametersParser;
        private readonly IPointGeneratorSettings pointGeneratorSettings;
        protected readonly IWordDataProvider wordDataProvider;
        private readonly IWordsExtractor wordsExtractor;
        private readonly IWordsTransformer wordsTransformer;

        public TagsCloudApp(IWordDataProvider wordDataProvider,
            IPointGeneratorSettings pointGeneratorSettings,
            ICloudParametersParser cloudParametersParser,
            IWordsExtractor wordsExtractor,
            IWordsTransformer wordsTransformer)
        {
            this.wordDataProvider = wordDataProvider;
            this.pointGeneratorSettings = pointGeneratorSettings;
            this.cloudParametersParser = cloudParametersParser;
            this.wordsExtractor = wordsExtractor;
            this.wordsTransformer = wordsTransformer;
        }

        public void Run(Options options)
        {
            cloudParametersParser.Parse(options)
                .Then(param => wordsExtractor.Extract(options.FilePath)
                    .Then(wordsTransformer.GetStems)
                    .Then(words => wordDataProvider
                        .GetData(new CircularCloudLayouter(param.PointGenerator, pointGeneratorSettings), words))
                    .Then(data => TagsCloudVisualizer.GetPicture(data, param))
                    .Then(bitmap => TagsCloudVisualizer.SavePicture(bitmap, param.OutFormat))
                    .OnFail(Console.WriteLine));
        }
    }
}