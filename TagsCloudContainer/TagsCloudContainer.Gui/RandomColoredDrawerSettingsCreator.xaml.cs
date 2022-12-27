using System;
using System.Windows;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

public partial class RandomColoredDrawerSettingsCreator : ISettingsCreator<RandomColoredDrawerSettings>
{
    private readonly IValidator<RandomColoredDrawerSettings> validator;

    public RandomColoredDrawerSettingsCreator(IValidator<RandomColoredDrawerSettings> validator)
    {
        this.validator = validator;
        InitializeComponent();
    }

    public RandomColoredDrawerSettings Settings { get; } = new();

    public Result<RandomColoredDrawerSettings> ShowCreate()
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