using Autofac;
using MyStemWrapper;
using TagsCloudContainer.CloudGenerators;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.FileProviders;
using TagsCloudContainer.Settings;
using TagsCloudContainer.TextAnalysers;
using TagsCloudContainer.TextAnalysers.WordsFilters;
using TagsCloudContainer.TextMeasures;
using TagsCloudContainer.Visualizers;

namespace TagsCloudContainer;

public class TagsCloudContainerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TagsCloudGenerator>().As<ITagsCloudGenerator>();
        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
        builder.RegisterType<FileReader>().As<IFileReader>();
        builder.RegisterType<ImageProvider>().As<IImageProvider>();
        builder.RegisterType<WordsFilter>().As<IWordsFilter>();
        builder.RegisterType<FrequencyCalculator>().As<IFrequencyCalculator>();
        builder.RegisterType<MyStemParser>().As<IMyStemParser>();
        builder.RegisterType<TextPreprocessor>().As<ITextPreprocessor>();
        builder.RegisterType<TagsCloudGenerator>().As<ITagsCloudGenerator>();
        builder.RegisterType<TagTextMeasurer>().As<ITagTextMeasurer>();
        builder.RegisterType<CloudVisualizer>().As<ICloudVisualizer>();
        builder.RegisterType<TagsCloudContainer>().As<ITagsCloudContainer>();

        builder.RegisterType<AppSettings>().As<IAppSettings>().SingleInstance();
        builder.RegisterType<AnalyseSettings>().As<IAnalyseSettings>().SingleInstance();
        builder.RegisterType<ImageSettings>().As<IImageSettings>().SingleInstance();
        builder.Register(context =>
            {
                var settings = context.Resolve<IAppSettings>();
                return new MyStem
                {
                    PathToMyStem = $@"{settings.ProjectDirectory}\mystem.exe",
                    Parameters = "-nli",
                };
            })
            .AsSelf();
    }
}