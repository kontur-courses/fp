using TagCloud;
using TagCloud.Configuration;
using TagCloud.Templates;
using TagCloud.TextHandlers;

namespace TagCloudApp.Apps;

public class ConsoleApp : IApp
{
    private readonly IReader reader;
    private readonly ITemplateCreator templateCreator;
    private readonly IVisualizer visualizer;
    private readonly Configuration configuration;

    public ConsoleApp(IReader reader, ITemplateCreator templateCreator, IVisualizer visualizer, Configuration configuration)
    {
        this.reader = reader;
        this.templateCreator = templateCreator;
        this.visualizer = visualizer;
        this.configuration = configuration;
    }

    public void Run()
    {
        reader
            .Read(configuration.WordsFilename)
            .Then(templateCreator.GetTemplate)
            .Then(visualizer.Draw)
            .Then(b => b.Save(configuration.OutputFilename))
            .OnFail(Console.WriteLine);
    }
}