using System;
using Autofac;
using TagCloudGenerator.Clients;
using TagCloudGenerator.Clients.CmdClient;
using TagCloudGenerator.Clients.VocabularyParsers;
using TagCloudGenerator.GeneratorCore;
using TagCloudGenerator.GeneratorCore.CloudVocabularyPreprocessors;
using TagCloudGenerator.GeneratorCore.TagClouds;

namespace TagCloudGenerator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var container = BuildContainer(args);
            var generatingCloudStatus = container.Resolve<CloudGenerator>().CreateTagCloudImage();

            if (!generatingCloudStatus.IsSuccess)
                Console.WriteLine(generatingCloudStatus.Error);
        }

        private static IContainer BuildContainer(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance(new CommandLineClient(args).GetOptions())
                .As<ITagCloudOptions<ITagCloud>>().SingleInstance();
            containerBuilder.RegisterInstance(new TxtVocabularyParser(null)).As<ICloudVocabularyParser>();
            containerBuilder.RegisterType<CloudContextGenerator>().As<ICloudContextGenerator>().SingleInstance();
            containerBuilder
                .RegisterInstance(new Func<TagCloudContext, CloudVocabularyPreprocessor>(PreprocessorConstructor))
                .As<Func<TagCloudContext, CloudVocabularyPreprocessor>>();
            containerBuilder.RegisterType<CloudGenerator>().AsSelf();

            return containerBuilder.Build();
        }

        private static CloudVocabularyPreprocessor PreprocessorConstructor(TagCloudContext cloudContext)
        {
            CloudVocabularyPreprocessor preprocessor = new ExcludingPreprocessor(null, cloudContext);
            preprocessor = new ToLowerPreprocessor(preprocessor);

            return preprocessor;
        }
    }
}