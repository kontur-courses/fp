using Autofac;
using TagCloudContainer.Configs;
using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;
using TagCloudContainer.Core.Utils;
using TagCloudContainer.Forms.Interfaces;
using TagCloudContainer.Forms.Validators;
using TagCloudContainer.Utils;

namespace TagCloudContainer.Forms;

public static class Register
{
    public static IContainer Registry()
    {
        var builder = new ContainerBuilder();
        var (selectedValues, tagCloudPlacerConfig, tagCloudContainerConfig) = GetSettingsInstancesWithDefaultValues();

        builder.RegisterType<TagCloud>();
        builder.RegisterType<Settings>();

        builder.RegisterInstance(tagCloudContainerConfig).As<ITagCloudContainerConfig>();
        builder.RegisterInstance(tagCloudPlacerConfig).As<ITagCloudPlacerConfig>();
        builder.RegisterInstance(selectedValues).As<ISelectedValues>();

        builder.RegisterType<TagCloudContainerConfigValidator>().As<IConfigValidator<ITagCloudContainerConfig>>();
        builder.RegisterType<TagCloudPlacerConfigValidator>().As<IConfigValidator<ITagCloudPlacerConfig>>();
        builder.RegisterType<SelectedValuesValidator>().As<IConfigValidator<ISelectedValues>>();
        builder.RegisterType<ImageCreator>().As<IImageCreator>().SingleInstance();
        builder.RegisterType<SizeInvestigator>().As<ISizeInvestigator>().SingleInstance();
        builder.RegisterType<LinesValidator>().As<ILinesValidator>().SingleInstance();
        builder.RegisterType<TagCloudPlacer>().As<ITagCloudPlacer>().SingleInstance();
        builder.RegisterType<WordsReader>().As<IWordsReader>().SingleInstance();
        builder.RegisterType<TagCloudProvider>().As<ITagCloudProvider>().SingleInstance();

        return builder
            .Build()
            .AsResult()
            .OnFail(error => Console.WriteLine(error))
            .GetValueOrThrow();
    }

    private static (ISelectedValues, ITagCloudPlacerConfig, ITagCloudContainerConfig) GetSettingsInstancesWithDefaultValues()
    {
        var availableColor = Colors.GetAll().First().Value;
        
        var tagCloudPlacerConfig = new TagCloudPlacerConfig()
        {
            FieldCenter = new Point(10, 10),
            NearestToTheFieldCenterPoints = new SortedList<float, Point>(),
            PutRectangles = new List<Rectangle>()
        };
        var selectedValues = new SelectedValues()
        {
            WordsColor = availableColor,
            BackgroundColor = availableColor,
            FontFamily = "Arial",
            PlaceWordsRandomly = false,
            ImageSize = Screens.Sizes.First()
        };
        
        var tagCloudContainerConfig = new TagCloudContainerConfig()
        {
            ImageName = "TagCloudResult.png",
        };
        
        var wordsFilePath = PathAssistant.GetFullFilePath("words.txt");
        var excludeWordsFilePath = PathAssistant.GetFullFilePath("boring_words.txt");

        tagCloudContainerConfig.WordsFilePath = wordsFilePath.IsSuccess ? wordsFilePath.GetValueOrThrow() : null;
        tagCloudContainerConfig.ExcludeWordsFilePath = excludeWordsFilePath.IsSuccess ? excludeWordsFilePath.GetValueOrThrow() : null;
        
        return (selectedValues, tagCloudPlacerConfig, tagCloudContainerConfig);
    }
}