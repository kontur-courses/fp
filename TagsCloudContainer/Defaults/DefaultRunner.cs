using Autofac;
using Mono.Options;
using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults.SettingsProviders;

namespace TagsCloudContainer.Defaults;

public class DefaultRunner : IRunner
{
    private readonly ILifetimeScope container;

    public DefaultRunner(ILifetimeScope container)
    {
        this.container = container;
    }

    public Result Run(params string[] args)
    {
        var parseSettingsResult = ParseSettings(args);
        if (!parseSettingsResult.IsSuccess)
        {
            return parseSettingsResult.
                RefineError($"Failed to parse arguments[{ string.Join(", ", args)}]");
        }

        var outputResult = Result.Of(container.Resolve<OutputSettings>);
        if (!outputResult.IsSuccess)
            return outputResult;

        var output = outputResult.GetValueOrThrow();
        return Result.Of(container.Resolve<IVisualizer>)
            .Then(vis => vis.GetBitmap())
            .Then(img => img.Save(output.OutputPath, output.ImageFormat.GetFormat()));
    }

    private Result ParseSettings(string[] args)
    {
        var allSettingsProviders = container.Resolve<IEnumerable<ICliSettingsProvider>>().ToList();
        var argsList = args.ToList();
        var allOptions = allSettingsProviders.Select(x => x.GetCliOptions()).ToList();
        var helperOptions = new OptionSet
        {
            { "h|?|help", "Show this help", (string v) => ShowHelp(allOptions) }
        };
        allOptions.Add(helperOptions);

        foreach (var item in allOptions)
        {
            argsList = item.Parse(argsList);
            if (!argsList.Any())
                break;
        }

        foreach (var provider in allSettingsProviders)
        {
            if (!provider.State.IsSuccess)
                return provider.State;
        }

        if (argsList.Any())
            return Result.Fail($"Unknown arguments encountered: [{string.Join(", ", argsList)}]");

        foreach (var provider in allSettingsProviders.OfType<IRequiredSettingsProvider>())
        {
            if (!provider.IsSet)
                return Result.Fail($"One of the required arguments were not provided: {provider.GetType().Name}");
        }

        return Result.Ok();
    }

    private static void ShowHelp(List<OptionSet> allOptions)
    {
        foreach (var option in allOptions)
        {
            option.WriteOptionDescriptions(Console.Out);
        }
    }
}
