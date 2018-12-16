using Autofac;


namespace TagsCloud
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = Options.Parse(args);
            var container = Ioc.Configure(options);
            var app = container.Resolve<Application>();
            app.Run(options);
        }
    }
}
