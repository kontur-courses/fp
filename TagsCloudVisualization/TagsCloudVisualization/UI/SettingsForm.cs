using System.ComponentModel;
using System.Windows.Forms;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.UI
{
    public class SettingsForm : Form
    {
        public ImageSettings ModifiedSettings { get; private set; }

        private readonly PropertyGrid propertyGrid;

        public SettingsForm(ImageSettings imageSettings)
        {
            ModifiedSettings = imageSettings;
            propertyGrid = new PropertyGrid
            {
                SelectedObject = imageSettings,
                Dock = DockStyle.Fill
            };
            Controls.Add(propertyGrid);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ModifiedSettings = (ImageSettings)propertyGrid.SelectedObject;
        }
    }
}