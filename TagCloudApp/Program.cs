using System.Diagnostics.CodeAnalysis;
using Autofac;
using TagCloud;
using TagCloudApp.Apps;
using TagCloudApp.Configurations;
using Configuration = TagCloudApp.Configurations.Configuration;


namespace TagCloudApp
{
    public static class Program
    {
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.Func`2[System.Drawing.RectangleF,System.Boolean]")]
        public static void Main(string[] args)
        {
            CommandLineConfigurationProvider.GetConfiguration(args)
                .Validate(c => c != null)
                .Then(BuildContainer)
                .Then(c => c.Resolve<IApp>())
                .Then(a => a.Run())
                .OnFail(Console.WriteLine);
        }

        private static IContainer BuildContainer(Configuration configuration)
        {
            var builder = new ContainerBuilder().GetDefaultBuild(configuration);
            builder.RegisterType<ConsoleApp>().As<IApp>();
            return builder.Build();
        }
    }
}