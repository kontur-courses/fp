using System;
using Autofac;

namespace TagsCloud
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var options = Options.Parse(args);
            if (!options.IsSuccess)
            {
                Console.WriteLine(options.Error);
                return;
            }

            var container = ContainerConstructor.Configure(options.Value);
            var app = container.Resolve<Application>();
            app.Run();
        }
    }
}