using CommandDotNet;
using CommandDotNet.IoC.Autofac;
using TagsCloudContainerCore.Helpers;
using WinCloudLayouterConsoleUI.DI;
using static WinCloudLayouterConsoleUI.WinConsoleUI;

namespace WinCloudLayouterConsoleUI;

public static class Program
{
    public static void Main(string[] args)
    {
        if (!File.Exists("./TagsCloudSettings.xml"))
        {
            JsonSettingsHelper.CreateSettingsFile();
        }

        var settings = JsonSettingsHelper.TryGetLayoutSettings();
        if (!settings.IsSuccess)
        {
            ExceptionHandle(settings.Error);
            return;
        }

        var container = DICloudLayouterContainerFactory.GetContainer(settings.GetValueOrThrow());
        if (!container.IsSuccess)
        {
            ExceptionHandle(container.Error);
            return;
        }

        new AppRunner<WinConsoleUI>()
            .UseAutofac(container.GetValueOrThrow())
            .Run(args);
    }
}