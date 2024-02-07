using TagsCloudVisualization.Common;
using TagsCloudVisualization.WFApp.Common;
using TagsCloudVisualization.WFApp.Factories;
using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp;

public class MainForm : Form
{
    public MainForm(IEnumerable<IUiAction> actions,
        PictureBoxImageHolder pictureBox,
        ImageSettings imageSettings,
        IToolStripItemFactory factory)
    {
        ClientSize = new Size(imageSettings.Width, imageSettings.Height);

        var mainMenu = new MenuStrip();
        mainMenu.Items.AddRange(actions.ToMenuItems(factory));
        Controls.Add(mainMenu);

        pictureBox.RecreateImage(imageSettings);
        pictureBox.Dock = DockStyle.Fill;
        Controls.Add(pictureBox);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Text = Resources.MainForm_OnShown_Name;
    }
}
