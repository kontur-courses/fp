using System.Reflection;
using Autofac;
using ConsoleApp.Handlers;
using ConsoleApp.Options;
using Module = Autofac.Module;

namespace ConsoleApp;

public class ConsoleAppModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = typeof(CommandLineParser).GetTypeInfo().Assembly;
        
        builder.RegisterType<CommandLineParser>().As<ICommandLineParser>();


        builder.RegisterAssemblyTypes(assembly)
            .Where(t => typeof(IOptionsHandler).IsAssignableFrom(t))
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => typeof(IOptions).IsAssignableFrom(t))
            .AsImplementedInterfaces();
    }
}