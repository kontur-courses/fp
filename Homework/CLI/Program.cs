using System;
using Autofac;
using ContainerConfigurers;
using TagsCloudContainer;

namespace CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = new Client(args).UserConfig;
            if (config == null) return;
            var container = new AutofacConfigurer(config).GetContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var painter = scope.Resolve<CloudPainter>();
                var paintResult = painter.Draw();
                Console.WriteLine(paintResult.GetValueOrThrow());
            }
        }
    }
}
