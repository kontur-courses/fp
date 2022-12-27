using System.Windows;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

public partial class ClassicDrawerSettingsCreator : ISettingsCreator<ClassicDrawerSettings>
{
    private readonly IValidator<ClassicDrawerSettings> validator;

    public ClassicDrawerSettingsCreator(IValidator<ClassicDrawerSettings> validator)
    {
        this.validator = validator;
        InitializeComponent();
    }

    private ClassicDrawerSettings Settings { get; } = new();

    Result<ClassicDrawerSettings> ISettingsCreator<ClassicDrawerSettings>.ShowCreate()
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