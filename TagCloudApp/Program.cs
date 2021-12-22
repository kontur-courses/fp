using Autofac;
using TagCloud;
using TagCloudApp.Apps;
using TagCloudApp.Configurations;
using Configuration = TagCloudApp.Configurations.Configuration;


namespace TagCloudApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var configuration = CommandLineConfigurationProvider.GetConfiguration(args);
            configuration
                .Validate(c => c != null)
                .Then(BuildContainer)
                .Then(c => c.Resolve<IApp>())
                .Then(a => a.Run(configuration.Value))
                .OnFail(Console.WriteLine);
        }

        private static IContainer BuildContainer(Configuration configuration)
        {
            var builder = ContainerBuilder.GetDefault(configuration);
            builder.RegisterType<ConsoleApp>().As<IApp>();
            return builder.Build();
        }
    }
}