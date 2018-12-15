using System.Windows.Forms;
using Autofac;
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
        private readonly IWordsExtractorSettings wordsExtractorSettings;
        private readonly IWordsTransformer wordsTransformer;

        public TagsCloudApp(IWordDataProvider wordDataProvider,
            IWordsExtractorSettings wordsExtractorSettings,
            IPointGeneratorSettings pointGeneratorSettings,
            ICloudParametersParser cloudParametersParser,
            IWordsExtractor wordsExtractor,
            IWordsTransformer wordsTransformer)
        {
            this.wordDataProvider = wordDataProvider;
            this.wordsExtractorSettings = wordsExtractorSettings;
            this.pointGeneratorSettings = pointGeneratorSettings;
            this.cloudParametersParser = cloudParametersParser;
            this.wordsExtractor = wordsExtractor;
            this.wordsTransformer = wordsTransformer;
        }

        public void Run(Options options, IContainer container)
        {
            cloudParametersParser.Parse(options)
                .Then(param => wordsExtractor.Extract(options.FilePath, wordsExtractorSettings)
                    .Then(words => wordsTransformer.GetStems(words))
                    .Then(words => wordDataProvider
                        .GetData(new CircularCloudLayouter(param.PointGenerator, pointGeneratorSettings), words)
                        .Then(data => TagsCloudVisualizer.GetPicture(data, param))
                        .Then(bitmap => TagsCloudVisualizer.SavePicture(bitmap, param.OutFormat))
                        .OnFail(error => MessageBox.Show(error, "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error))));
        }
    }
}