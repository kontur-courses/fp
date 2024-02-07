using Autofac;
using TagCloud.UserInterface;

namespace TagCloud;

public class Program
{
    static void Main(string[] args)
    {
        var settings = Configurator.Parse(args);

        Configurator.ConfigureBuilder(settings)
            .Build()
            .Resolve<IUserInterface>()
            .Run(settings);
    }
}