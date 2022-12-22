using Autofac;
using TagCloudContainer.Additions;
using TagCloudContainer.Additions.Interfaces;
using TagCloudContainer.Additions.Models;
using TagCloudContainer.Forms;
using TagCloudContainer.Utils;

namespace TagCloudContainer;

public static class Register
{
    public static IContainer Registry()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<TagCloud>();
        builder.RegisterType<Settings>();

        var tagCloudContainerConfig = new TagCloudContainerConfig();
        var tagCloudFormConfig = new TagCloudFormConfig();
        var wordReaderConfig = new WordReaderConfig();
        var tagCloudFormResult = new TagCloudFormResult();

        builder.RegisterInstance(tagCloudContainerConfig).As<ITagCloudContainerConfig>();
        builder.RegisterInstance(tagCloudFormConfig).As<ITagCloudFormConfig>();
        builder.RegisterInstance(wordReaderConfig).As<IWordReaderConfig>();
        builder.RegisterInstance(tagCloudFormResult).As<ITagCloudFormResult>();

        builder.RegisterType<Validator>().AsSelf();
        builder.RegisterType<ImageCreator>().As<IImageCreator>().SingleInstance();
        builder.RegisterType<SizeInvestigator>().As<ISizeInvestigator>().SingleInstance();
        builder.RegisterType<WordValidator>().As<IWordValidator>().SingleInstance();
        builder.RegisterType<TagCloudPlacer>().As<ITagCloudPlacer>().SingleInstance();
        builder.RegisterType<WordsReader>().As<IWordsReader>().SingleInstance();
        builder.RegisterType<TagCloudProvider>().As<ITagCloudProvider>().SingleInstance();
        
        return builder.Build();
    }
}