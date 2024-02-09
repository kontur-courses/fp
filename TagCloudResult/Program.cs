using Autofac;
using CommandLine;
using TagCloudResult.Applications;

namespace TagCloudResult
{
    abstract class MainClass
    {
        public static void Main(string[] args)
        {
            var settingsResult = Parser.Default.ParseArguments<Settings>(args);
            if (settingsResult.Errors.Any())
                return;
            var appResult = Result<IApplication>
                .Of(() => Container.SetupContainer(settingsResult.Value).Resolve<IApplication>())
                .RefineError("Impossible to build app")
                .Then(x => x.Run());
            if (!appResult.IsSuccess)
                Console.WriteLine(appResult.Error);
        }
    }
}
