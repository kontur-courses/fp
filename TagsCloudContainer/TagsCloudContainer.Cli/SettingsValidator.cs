using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Cli;

public class SettingsValidator
{
    private readonly IValidator<CircularLayouterAlgorithmSettings> circularLayouterAlgorithmSettingsValidator;
    private readonly IValidator<ClassicDrawerSettings> classicDrawerSettingsValidator;
    private readonly IValidator<CliGraphicsProviderSettings> cliGraphicsProviderSettingsValidator;
    private readonly IValidator<RandomColoredDrawerSettings> randomColoredDrawerSettingsValidator;
    private readonly Settings settings;

    public SettingsValidator(IValidator<ClassicDrawerSettings> classicDrawerSettingsValidator,
        IValidator<RandomColoredDrawerSettings> randomColoredDrawerSettingsValidator,
        IValidator<CircularLayouterAlgorithmSettings> circularLayouterAlgorithmSettingsValidator,
        IValidator<CliGraphicsProviderSettings> cliGraphicsProviderSettingsValidator,
        Settings settings)
    {
        this.classicDrawerSettingsValidator = classicDrawerSettingsValidator;
        this.randomColoredDrawerSettingsValidator = randomColoredDrawerSettingsValidator;
        this.circularLayouterAlgorithmSettingsValidator = circularLayouterAlgorithmSettingsValidator;
        this.cliGraphicsProviderSettingsValidator = cliGraphicsProviderSettingsValidator;
        this.settings = settings;
    }

    public Result Validate()
    {
        return Result.Combine(
                settings.DrawerSettings.OfType<ClassicDrawerSettings>().Select(classicDrawerSettingsValidator.Validate))
            .CombineCheck(() => Result.Combine(settings.DrawerSettings.OfType<RandomColoredDrawerSettings>()
                .Select(randomColoredDrawerSettingsValidator.Validate)))
            .CombineCheck(() => Result.Combine(settings.LayouterAlgorithmSettings
                .OfType<CircularLayouterAlgorithmSettings>()
                .Select(circularLayouterAlgorithmSettingsValidator.Validate)))
            .CombineCheck(() => settings.GraphicsProviderSettings
                .SuccessIfCast<GraphicsProviderSettings, CliGraphicsProviderSettings>()
                .Bind(value => cliGraphicsProviderSettingsValidator.Validate(value)));
    }
}