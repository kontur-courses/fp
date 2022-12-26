using System.ComponentModel;
using System.Windows;

namespace TagsCloudContainer.Gui;

public partial class GuiGraphicsProviderSettingsEditor : ISettingsEditor<GuiGraphicsProviderSettings>,
    INotifyPropertyChanged
{
    public GuiGraphicsProviderSettingsEditor()
    {
        InitializeComponent();
    }

    private GuiGraphicsProviderSettings Settings { get; set; } = new();

    GuiGraphicsProviderSettings ISettingsEditor<GuiGraphicsProviderSettings>.ShowEdit(
        GuiGraphicsProviderSettings settings)
    {
        Settings = new()
        {
            Height = settings.Height,
            Width = settings.Width,
            Save = settings.Save,
            SavePath = settings.SavePath
        };
        DataContext = Settings;
        var result = ShowDialog() ?? false;
        return result ? Settings : settings;
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Submit(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}