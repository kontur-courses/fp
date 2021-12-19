using System;
using Autofac;
using TagsCloudContainer;

namespace TagsCloud.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            var appSettings = AppSettings.Parse(args)
                .Then(appSettings => builder.RegisterInstance(appSettings).AsImplementedInterfaces().SingleInstance())
                .Then(_ => TagCloudSettings.Parse(args))
                .Then(tagSettings => builder.RegisterInstance(tagSettings).AsImplementedInterfaces().SingleInstance())
                .OnFail(HandleError);
            builder.RegisterType<ConsoleUI>().AsImplementedInterfaces();
            builder.RegisterModule<InfrastructureModule>();
            using var container = builder.Build();
            container.Resolve<IConsoleUI>()
                .Run()
                .OnFail(HandleError);
        }

        private static void HandleError(string error)
        {
            System.Console.WriteLine(error);
            Environment.Exit(-1);
        }
    }
}