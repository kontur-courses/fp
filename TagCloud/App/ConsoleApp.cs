using System;
using TagCloud.AppConfiguration;
using TagCloud.FileReader;
using TagCloud.FrequencyAnalyzer;
using TagCloud.ImageProcessing;
using TagCloud.ResultMonade;
using TagCloud.TextParsing;
using TagCloud.WordConverter;
using TagCloud.WordFilter;

namespace TagCloud.App
{
    public class ConsoleApp : IApp
    {
        private readonly IFileReader fileReader;
        private readonly ITextParser textParser;
        private readonly IConvertersExecutor convertersExecutor;
        private readonly IFiltersExecutor filtersExecutor;
        private readonly IWordsFrequencyAnalyzer wordsFrequencyAnalyzer;
        private readonly ICloudImageGenerator cloudImageGenerator;
        private readonly IAppConfig appConfig;

        public ConsoleApp(IFileReader fileReader,
                          ITextParser textParser,
                          IConvertersExecutor convertersExecutor,
                          IFiltersExecutor filtersExecutor,
                          IWordsFrequencyAnalyzer wordsFrequencyAnalyzer,
                          ICloudImageGenerator cloudImageGenerator,
                          IAppConfig appConfig)
        {
            this.fileReader = fileReader;
            this.textParser = textParser;
            this.convertersExecutor = convertersExecutor;
            this.filtersExecutor = filtersExecutor;
            this.wordsFrequencyAnalyzer = wordsFrequencyAnalyzer;
            this.cloudImageGenerator = cloudImageGenerator;
            this.appConfig = appConfig;
        }

        public void Run()
        {
            var g = fileReader.ReadAllText(appConfig.InputTextFilePath)
                              .Then(textParser.GetWords)
                              .Then(convertersExecutor.Convert)
                              .Then(filtersExecutor.Filter)
                              .Then(wordsFrequencyAnalyzer.GetFrequencies)
                              .Then(cloudImageGenerator.GenerateBitmap)
                              .Then( b => ImageSaver.SaveBitmap(b, appConfig.OutputImageFilePath))
                              .OnFail(Console.WriteLine);
        }
    }
}
