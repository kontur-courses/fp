using System.Windows;

namespace TagsCloudContainer.Gui;

public partial class ClassicDrawerSettingsCreator : ISettingsCreator<ClassicDrawerSettings>
{
    public ClassicDrawerSettingsCreator()
    {
        InitializeComponent();
    }

    private ClassicDrawerSettings Settings { get; } = new();

    ClassicDrawerSettings? ISettingsCreator<ClassicDrawerSettings>.ShowCreate()
    {
        DataContext = Settings;
        var result = ShowDialog() ?? false;
        return result ? Settings : null;
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