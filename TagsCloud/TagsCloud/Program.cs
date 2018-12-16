using System;
using Autofac;


namespace TagsCloud
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = Options.Parse(args);
            if (!options.IsSuccess)
            {
                Console.WriteLine(options.Error);
                return;
            }
            var container = Ioc.Configure(options.Value);
            var app = container.Resolve<Application>();
            app.Run(options.Value);
        }
    }
}
