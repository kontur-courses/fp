using LightInject;
using TagsCloudApp.ConsoleActions;
using TagsCloudApp.ConsoleInterface;
using TagsCloudContainer.Appliers;
using TagsCloudContainer.BitmapSavers;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Parsers;
using TagsCloudContainer.Spirals;
using TagsCloudContainer.TagCloudPainters;
using TagsCloudContainer.TagCreators;
using TagsCloudContainer.TagPainters;
using TagsCloudContainer.TagsCloudLayouter;

namespace TagsCloudApp.Providers;

public static class ContainerProvider
{
    public static ServiceContainer GetContainer()
    {
        var container = new ServiceContainer();
        container.RegisterInstance(container);
        container.Register<IParser, TxtParser>("1");
        container.Register<IParser, DocParser>("2");
        container.Register<IPreprocessorsApplier, PreprocessorsApplier>();
        container.Register<IFiltersApplier, FiltersApplier>();
        container.Register<ITagCreator, TagCreator>();
        container.Register<ICloudLayouter, OvalCloudLayouter>();
        container.Register<ICloudTagCreator, CloudTagCreator>();
        container.Register<ITagCloudPainter, TagCloudPainter>();
        container.Register<IBitmapSaver, CloudBitmapSaver>();

        container.RegisterInstance(SettingsProvider.GetSettings());
        RegisterImageSettings(container);
        RegisterCloudSettings(container);
        RegisterConsole(container);
        return container;
    }

    private static void RegisterConsole(ServiceContainer container)
    {
        container.Register<IUIAction, ImageSettingsAction>("1");
        container.Register<IUIAction, CloudSettingsAction>("2");
        container.Register<IUIAction, GenerateImageAction>("3");
        container.Register<IUI, ConsoleUI>();
    }

    private static void RegisterImageSettings(ServiceContainer container)
    {
        container.RegisterInstance(ImageSettingsProvider.GetColors());
        container.RegisterInstance(ImageSettingsProvider.GetFonts());
        container.RegisterInstance(ImageSettingsProvider.GetStyles());
        container.RegisterInstance(ImageSettingsProvider.GetFormats());
    }

    private static void RegisterCloudSettings(ServiceContainer container)
    {
        container.RegisterInstance(CloudSettingsProvider.GetSettings());
        container.Register<ISpiral, ArchimedeanSpiral>("1");
        container.Register<ISpiral, OvalSpiral>("2");
        container.Register<ITagPainter, PrimaryTagPainter>("1");
        container.Register<ITagPainter, FrequencyTagPainter>("2");
        container.Register<ITagPainter, GradientTagPainter>("3");
        container.Register<ITagPainter, RandomTagPainter>("4");
    }
}