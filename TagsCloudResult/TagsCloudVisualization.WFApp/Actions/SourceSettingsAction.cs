using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp.Actions;

public class SourceSettingsAction : IUiAction
{
    private readonly SourceSettings sourceSettings;

    public SourceSettingsAction(SourceSettings sourceSettings)
    {
        this.sourceSettings = sourceSettings;
    }

    public MenuCategory Category => MenuCategory.Settings;
    public string Name => Resources.SourceSettingsAction_Name;
    public string Description => Resources.SourceSettingsAction_Description;

    public void Perform()
    {
        var dialog = new OpenFileDialog()
        {
            CheckFileExists = false,
            InitialDirectory = Path.GetFullPath("/"),
            DefaultExt = Resources.SourceSettingsAction_Perform_DefaultExt,
            Filter = Resources.SourceSettingsAction_Perform_Filter 
        };
        var res = dialog.ShowDialog();

        if (res == DialogResult.OK)
        {
            sourceSettings.Path = dialog.FileName;
        }
    }
}
