using System.Windows;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

public partial class GuiGraphicsProviderSettingsEditor : ISettingsEditor<GuiGraphicsProviderSettings>
{
    private readonly IValidator<GuiGraphicsProviderSettings> validator;

    public GuiGraphicsProviderSettingsEditor(IValidator<GuiGraphicsProviderSettings> validator)
    {
        this.validator = validator;
        InitializeComponent();
    }

    private GuiGraphicsProviderSettings Settings { get; set; } = new();

    GuiGraphicsProviderSettings ISettingsEditor<GuiGraphicsProviderSettings>.ShowEdit(
        GuiGraphicsProviderSettings input)
    {
        Settings = new()
        {
            Height = input.Height,
            Width = input.Width,
            Save = input.Save,
            SavePath = input.SavePath
        };
        DataContext = Settings;
        var result = ShowDialog() ?? false;
        return result ? Settings : input;
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