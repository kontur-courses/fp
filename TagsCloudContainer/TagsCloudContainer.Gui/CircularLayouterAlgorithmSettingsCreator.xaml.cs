using System.Windows;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

public partial class CircularLayouterAlgorithmSettingsCreator : ISettingsCreator<CircularLayouterAlgorithmSettings>
{
    private readonly IValidator<CircularLayouterAlgorithmSettings> validator;

    public CircularLayouterAlgorithmSettingsCreator(IValidator<CircularLayouterAlgorithmSettings> validator)
    {
        this.validator = validator;
        InitializeComponent();
    }

    private CircularLayouterAlgorithmSettings Settings { get; } = new();

    Result<CircularLayouterAlgorithmSettings> ISettingsCreator<CircularLayouterAlgorithmSettings>.ShowCreate()
    {
        DataContext = Settings;
        return Result.SuccessIf(ShowDialog() ?? false, Settings, string.Empty);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Submit(object sender, RoutedEventArgs e)
    {
        validator.Validate(Settings)
            .Tap(() => DialogResult = true)
            .Tap(Close)
            .TapError(errors => MessageBox.Show(this, errors, "Errors", MessageBoxButton.OK, MessageBoxImage.Error));
    }
}