using Autofac;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.PointsProviders;
using TagsCloudVisualization.TagProviders;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.WFApp.Common;
using TagsCloudVisualization.WFApp.Factories;
using TagsCloudVisualization.WFApp.Infrastructure;
using TagsCloudVisualization.WordsProcessors;

namespace TagsCloudVisualization.WFApp;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        var container = new ContainerBuilder();
        
        //Регистрация всех действий, отображаемых в приложении. Подключает все наследуемые от IUiAction классы.
        container.RegisterAssemblyTypes(typeof(IUiAction).Assembly).AsImplementedInterfaces();
        
        //Регистрация всех настроечных классов и класса изображений, как Singleton.
        container.RegisterType<PictureBoxImageHolder>().As<PictureBoxImageHolder, IImageHolder>().SingleInstance();
        container.RegisterType<ImageSettings>().SingleInstance();
        container.RegisterType<SourceSettings>().SingleInstance();
        container.RegisterType<ArchimedeanSpiralSettings>().SingleInstance();
        container.RegisterType<TagsSettings>().SingleInstance();

        //Регистрация всех остальных зависимостей приложения, используемых основным проектом.
        container.RegisterType<SimpleWordsProcessor>().As<IWordsProcessor>();
        container.RegisterType<TxtTextReader>().Named<ITextReader>(".txt");
        container.RegisterType<DocTextReader>().Named<ITextReader>(".doc").Named<ITextReader>(".docx");
        container.RegisterType<DefaultTopLevelMenuItemFactory>().As<ITopLevelMenuItemFactory>();
        container.RegisterType<TextReaderFactory>().As<ITextReaderFactory>();
        container.RegisterType<TagProvider>().As<ITagProvider>();
        container.RegisterType<CircularCloudLayouter>().As<ITagsCloudLayouter>();
        container.RegisterType<ArchimedeanSpiralPointsProvider>();
        container.RegisterType<TagsCloudVisualizator>();
        
        //Регистрация основной формы приложения.
        container.RegisterType<MainForm>();
        
        ApplicationConfiguration.Initialize();
        Application.Run(container.Build().Resolve<MainForm>());
    }
}
