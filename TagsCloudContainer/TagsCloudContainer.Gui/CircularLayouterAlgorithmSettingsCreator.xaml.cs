using System.Windows;

namespace TagsCloudContainer.Gui;

public partial class CircularLayouterAlgorithmSettingsCreator : ISettingsCreator<CircularLayouterAlgorithmSettings>
{
    public CircularLayouterAlgorithmSettingsCreator()
    {
        InitializeComponent();
    }

    private CircularLayouterAlgorithmSettings Settings { get; } = new();

    CircularLayouterAlgorithmSettings? ISettingsCreator<CircularLayouterAlgorithmSettings>.ShowCreate()
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