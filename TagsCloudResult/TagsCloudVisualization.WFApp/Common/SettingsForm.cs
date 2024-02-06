using System.ComponentModel.DataAnnotations;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.WFApp.Common;

public static class SettingsForm
{
    public static SettingsForm<TSettings> For<TSettings>(TSettings settings)
        where TSettings : ISettings<TSettings>
    {
        return new SettingsForm<TSettings>(settings);
    }
}

public class SettingsForm<TSettings> : Form
    where TSettings : ISettings<TSettings>
{
    public SettingsForm(TSettings settings)
    {
        var okButton = new Button
        {
            Text = Resources.Text_OK,
            DialogResult = DialogResult.OK,
            Dock = DockStyle.Bottom,
        };
        var propertyGrid = new PropertyGrid
        {
            SelectedObject = settings,
            Dock = DockStyle.Fill
        };
        propertyGrid.Validating += (_, args) => settings.Validate().OnFail(x =>
        {
            args.Cancel = true;
            throw new ValidationException(x);
        });
        Controls.Add(okButton);
        Controls.Add(propertyGrid);
        AcceptButton = okButton;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = Resources.SettingsForm_OnLoad_Text;
    }
}
