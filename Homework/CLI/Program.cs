using System;
using Autofac;
using ContainerConfigurers;
using TagsCloudContainer;
using TagsCloudContainer.ClientsInterfaces;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Client(args).UserConfig;
            if (config == null) return;
            var container = new AutofacConfigurer(config).GetContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var painter = scope.Resolve<CloudPainter>();
                painter.Draw();
            }
        }
    }
}
