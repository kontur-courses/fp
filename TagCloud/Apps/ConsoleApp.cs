using System;
using System.Linq;
using TagCloud.Configurations;
using TagCloud.Templates;
using TagCloud.TextHandlers;

namespace TagCloud.Apps
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
            /*var words = reader.Read(configuration.WordsFilename);
            if (words.Length == 0)
            {
                Console.WriteLine($"File {configuration.WordsFilename} is empty");
                return;
            }*/

            reader
                .Read(configuration.WordsFilename)
                .Then(templateCreator.GetTemplate)
                .Then(t=>visualizer.Draw(t, configuration.OutputFilename));
        }
    }
}