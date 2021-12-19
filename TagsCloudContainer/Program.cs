using LightInject;
using TagsCloudContainer.ConsoleInterface;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.Providers;

namespace TagsCloudContainer
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
