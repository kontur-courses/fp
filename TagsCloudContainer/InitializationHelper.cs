using Autofac;
using Mono.Options;
using ResultExtensions;
using ResultOf;
using System.Reflection;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer;

public static class InitializationHelper
{
    private const string allCase = "all";
    private const string noneCase = "none";

    public static Result RunWithArgs(string[] args)
    {
        var registeredServices = new HashSet<Type>();
        var serviceIndex = new ServiceIndex();
        var builder = new ContainerBuilder();

        serviceIndex.AddAssemblyTypes(Assembly.GetExecutingAssembly());
        var leftArgs = ParseAssemblies(serviceIndex, args)
            .Then(x => ParseImplementations(serviceIndex, builder, registeredServices, x))
            .Then(x => RegisterNotSpecified(builder, serviceIndex, registeredServices));

        return leftArgs.Then(_ => builder.Build())
            .Then(container => container.Resolve<IRunner>())
            .Then(runner => runner.Run(leftArgs.GetValueOrThrow().ToArray()))
            .RefineError("Failed to run");
    }

    private static void RegisterNotSpecified(ContainerBuilder builder, ServiceIndex serviceIndex, HashSet<Type> registeredServices)
    {
        foreach (var impl in serviceIndex.AvailableImplementations)
        {
            var registration = builder.RegisterType(impl.Implementation).AsSelf();
            if (impl.IsSingleton)
                registration.SingleInstance();
            foreach (var possibleService in impl.ImplementedServices)
            {
                if (registeredServices.Contains(possibleService))
                    continue;
                registration.As(possibleService);
            }
        }
    }

    private static Result<List<string>> ParseImplementations(ServiceIndex serviceIndex, ContainerBuilder builder, HashSet<Type> registeredServices, List<string> leftArgs)
    {
        Result result = default;
        var implementationsOptions = new OptionSet()
        {
            {"implement-with=",$"Specifies which implementations to register for later use. " +
            $"Example: '--implement-with IService FirstImpl SecondImpl' will register class FirstImpl and SecondImpl as implementations for IService. " +
            $"Special cases: '{allCase}' - register all available implementations of service (default behaviour), '{noneCase}' - not register any implmentations for service" ,
                v => result = RegisterFromArgs(builder,serviceIndex, registeredServices, v) }
        };
        leftArgs = implementationsOptions.Parse(leftArgs);

        return result.Then(() => leftArgs);
    }

    private static Result<List<string>> ParseAssemblies(ServiceIndex serviceIndex, string[] args)
    {
        Result result = default;
        var assembliesOptions = new OptionSet()
        {
            {"assemblies=",$"Specifies additional assemblies to use.",v => result = AddAssembliesFrom(serviceIndex,v.Split()) }
        };
        var leftArgs = assembliesOptions.Parse(args);

        return result.Then(() => leftArgs);
    }

    private static Result RegisterFromArgs(ContainerBuilder builder, ServiceIndex serviceIndex, HashSet<Type> registeredServices, string argString)
    {
        if (argString == allCase)
            return Result.Ok();

        var types = argString.Split(' ');
        if (types.Length < 2)
            return Result.Fail($"{argString} did not provide enough types to register. Should be at least 2: Service and it's Implementation");
        var service = serviceIndex.GetService(types[0]);
        if (!service.IsSuccess)
            return service;
        if (types[1] == noneCase)
        {
            registeredServices.Add(service.GetValueOrThrow());
            return Result.Ok();
        }

        var impls = new List<ImplementationCard>();
        var implsResults = types.Skip(1).Select(serviceIndex.GetImplementation);
        foreach (var result in implsResults)
        {
            if (!result.IsSuccess)
                return result;
            impls.Add(result.GetValueOrThrow());
        }

        return Result.OfAction(() => Register(builder, service.GetValueOrThrow(), impls.ToArray()))
            .Then(() => registeredServices.Add(service.GetValueOrThrow()))
            .RefineError($"Could not register service from '{argString}'");

    }

    private static void Register(ContainerBuilder builder, Type service, ImplementationCard[] implemetations)
    {
        foreach (var impl in implemetations)
        {
            if (!service.IsAssignableFrom(impl.Implementation))
                throw new ArgumentException($"Implementation '{impl.Implementation.Name}' does not implement service '{service.Name}' interface");
            var registration = builder.RegisterType(impl.Implementation).AsSelf().As(service);
            if (impl.IsSingleton)
                registration.SingleInstance();
        }
    }

    private static Result AddAssembliesFrom(ServiceIndex serviceIndex, IEnumerable<string> assemblyNames)
    {
        foreach (var assembly in assemblyNames.Select(x => Result.Of(() => Assembly.LoadFrom(x))))
        {
            if (!assembly.IsSuccess)
                return Result.Fail($"Could not load assembly. {assembly.Error}");
            serviceIndex.AddAssemblyTypes(assembly.GetValueOrThrow());
        }

        return Result.Ok();
    }
}
