using System;
using Autofac;
using TagsCloudResult.ResultFormatters;

namespace TagsCloudResult
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            Container = new Ioc().GetContainer();

            using (var scope = Container.BeginLifetimeScope())
            {
                Console.WriteLine(scope.Resolve<IResultFormatter>().GenerateResult("tag-cloud.png").Error);
            }
        }
    }
}
