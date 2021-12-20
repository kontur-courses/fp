using LightInject;
using TagsCloudApp.ConsoleInterface;
using TagsCloudApp.Providers;

namespace TagsCloudApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerProvider.GetContainer();
            var ui = container.GetInstance<IUI>();
            ui.Run();
        }
    }
}
