using System;
using System.IO;
using System.Linq;
using Autofac;
using TagsCloudContainer.Configuration;
using TagsCloudContainer.Controller;
using TagsCloudContainer.DataReader;
using TagsCloudContainer.ResultOf;
using TagsCloudContainerCLI.CommandLineParser;

namespace TagsCloudContainerCLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new SimpleCommandLineParser().Parse(args);

            configuration
                .Then(BuildContainer)
                .Then(container => container.Resolve<ITagsCloudController>())
                .Then(tagsCloudController => tagsCloudController.Save())
                .ThenAction(() => configuration.Then(config =>
                    Console.WriteLine("Visualization has been saved to " +
                                      Path.Combine(config.DirectoryToSave,
                                          $"{config.OutFileName}.{config.ImageFormat}"))))
                .Then(none => Console.ReadKey())
                .OnFail(Console.WriteLine);
        }

        private static IContainer BuildContainer(IConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(configuration)
                .As<IConfiguration>();

            builder.RegisterInstance(new FileReader())
                .As<IDataReader>();

            var dataAccess = AppDomain.CurrentDomain.GetAssemblies()
                .First(assembly => assembly.FullName.Contains("TagsCloudContainer,"));

            builder.RegisterAssemblyTypes(dataAccess)
                .AsImplementedInterfaces()
                .Except<SimpleCommandLineParser>()
                .Except<SimpleConfiguration>()
                .Except<IDataReader>();

            return builder.Build();
        }
    }
}