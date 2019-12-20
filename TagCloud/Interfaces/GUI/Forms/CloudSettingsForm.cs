using System;
using System.Drawing;
using System.Windows.Forms;
using TagCloud.CloudLayouter;
using TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace;

namespace TagCloud.Interfaces.GUI.Forms
{
    class CloudSettingsForm : Form
    {
        private CloudViewConfiguration cloudViewConfiguration;
        private CloudConfiguration cloudConfiguration;

        public CloudSettingsForm(CloudViewConfiguration cloudViewConfiguration, CloudConfiguration cloudConfiguration)
        {
            this.cloudViewConfiguration = cloudViewConfiguration;
            this.cloudConfiguration = cloudConfiguration;
            InitializeForm();
        }

        private void InitializeForm()
        {
            Size = new Size(300, 500);
            Controls.Add(new PropertyGrid
            {
                SelectedObject = cloudConfiguration,
                Size = new Size(300, 200)
            });
            Controls.Add(new PropertyGrid
            {
                SelectedObject = cloudViewConfiguration,
                Size = new Size(300, 200),
                Location = new Point(0, 200)
            });

            var fontFamily = new Button
            {
                Text = "Выбрать шрифт",
                Dock = DockStyle.Bottom
            };
            fontFamily.Click += HandleFontFamilyButton;

            Controls.Add(fontFamily);

            Controls.Add(new Button {Text = "OK", Dock = DockStyle.Bottom, DialogResult = DialogResult.OK});
        }

        private void HandleFontFamilyButton(object sender, EventArgs e)
        {
            var dialog = new FontDialog();

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                cloudViewConfiguration.FontFamily = dialog.Font.FontFamily;
            }
        }
    }
}
