using System;
using TagCloud;
using TagCloud.Templates;
using TagCloud.TextHandlers;
using TagCloudApp.Configurations;

namespace TagCloudApp.Apps
{
    public class ConsoleApp : IApp
    {
        private readonly IReader reader;
        private readonly ITemplateCreator templateCreator;
        private readonly IVisualizer visualizer;

        public ConsoleApp(IReader reader, ITemplateCreator templateCreator, IVisualizer visualizer)
        {
            this.reader = reader;
            this.templateCreator = templateCreator;
            this.visualizer = visualizer;
        }

        public void Run(Configuration configuration)
        {
            reader
                .Read(configuration.WordsFilename)
                .Then(templateCreator.GetTemplate)
                .Then(t => visualizer.Draw(t))
                .Then(b => b.Save(configuration.OutputFilename))
                .OnFail(Console.WriteLine);
        }
    }
}