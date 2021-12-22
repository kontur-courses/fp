using System;
using System.IO;
using Autofac;
using TagsCloudVisualization.Commands;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.FileReaders;
using TagsCloudVisualization.Common.ImageWriters;
using TagsCloudVisualization.Common.TagCloudPainters;
using TagsCloudVisualization.Common.Tags;
using TagsCloudVisualization.Common.TextAnalyzers;

namespace TagsCloudVisualization.Processors
{
    public class ShowDemoProcessor : CommandProcessorBase<ShowDemoCommand>
    {
        private static readonly string[] TestFiles =
        {
            @"\demo\Test_Облако.txt",
            @"\demo\Test_Литературный_текст.txt",
            @"\demo\Text_Большой_текст.txt"
        };
        
        public override int Run(ShowDemoCommand options)
        {
            var executingPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            var result = Result.Ok();
            foreach (var testFile in TestFiles)
            {
                var saveFilePath = options.OutputPath + Path.GetFileNameWithoutExtension(testFile) + ".png";
                result.Then(_ => container.Resolve<IFileReader>().ReadFile(executingPath + testFile))  
                    .Then(text => container.Resolve<ITextAnalyzer>().GetWordStatistics(text))
                    .Then(stat => container.Resolve<ITagBuilder>().GetTags(stat))
                    .Then(tags => container.Resolve<ITagCloudPainter>().Paint(tags))
                    .Then(bitmap => container.Resolve<IImageWriter>().Save(bitmap, saveFilePath))
                    .OnSuccess(_ => Console.WriteLine($"Облако тегов сгенерировано и сохранено '{saveFilePath}'."))
                    .OnFail(Console.WriteLine);

                if (!result.IsSuccess)
                    return 1;
            }
            return 0;
        }
    }
}