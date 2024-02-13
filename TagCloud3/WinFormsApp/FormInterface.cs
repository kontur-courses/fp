using ResultOf;
using TagsCloudContainer.Drawer;
using TagsCloudContainer.FrequencyAnalyzers;
using TagsCloudContainer.SettingsClasses;
using TagsCloudContainer.TagCloudBuilder;
using TagsCloudContainer.TextTools;
using TagsCloudContainer.Visualizer;
using TagsCloudVisualization;
using WinFormsApp.SettingsForms;

namespace WinFormsApp
{
    internal class FormInterface : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem colorToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenu;

        private Graphics gr;
        private Result<Image> image;

        private AppSettings appSettings;
        private List<IPointsProvider> providers;

        public FormInterface()
        {
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem("File");
            openToolStripMenuItem = new ToolStripMenuItem("Open..");
            saveToolStripMenuItem = new ToolStripMenuItem("Save..");
            exitToolStripMenuItem = new ToolStripMenuItem("Exit");
            settingsToolStripMenuItem = new ToolStripMenuItem("Settings");
            colorToolStripMenuItem = new ToolStripMenuItem("Colors");
            propertiesToolStripMenu = new ToolStripMenuItem("Properties");

            menuStrip.Items.Add(fileToolStripMenuItem);
            menuStrip.Items.Add(settingsToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(openToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(saveToolStripMenuItem);
            fileToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            fileToolStripMenuItem.DropDownItems.Add(exitToolStripMenuItem);

            settingsToolStripMenuItem.DropDownItems.Add(colorToolStripMenuItem);
            settingsToolStripMenuItem.DropDownItems.Add(propertiesToolStripMenu);

            Controls.Add(menuStrip);

            openToolStripMenuItem.Click += new EventHandler(openToolStripMenuItem_Click);
            saveToolStripMenuItem.Click += new EventHandler(saveToolStripMenuItem_Click);
            colorToolStripMenuItem.Click += new EventHandler(colorToolStripMenuItem_Click);
            exitToolStripMenuItem.Click += new EventHandler(exitToolStripMenuItem_Click);
            propertiesToolStripMenu.Click += new EventHandler(propertiesToolStripMenuItem_Click);

            gr = this.CreateGraphics();

            appSettings = SettingsManager.SettingsManager.LoadSettings();

            providers = new List<IPointsProvider>()
            {
                new NormalPointsProvider(new Point(300, 300)),
                new SpiralPointsProvider(new Point(300, 300)),
                new RandomPointsProvider(new Point(300, 300))
            };
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                appSettings.TextFile = openDialog.FileName;

                var res = RebuildImage();

                if (!res.IsSuccess)
                {
                    ErrorMessageBox.ShowError(res.Error);
                }
                else
                {
                    RedrawImage(res);
                }
            }
        }

        private void RedrawImage(Result<Image> image)
        {
            if (image.IsSuccess)
            {
                Size = appSettings.DrawingSettings.Size;
                gr = this.CreateGraphics();
                gr.DrawImage(image.Value, new Point(0, 0));
                SettingsManager.SettingsManager.SaveSettings(appSettings);
            }
            else
            {
                ErrorMessageBox.ShowError(image.Error);
            }
        }

        private Result<Image> RebuildImage()
        {
            var rawText = TextFileReader.ReadText(appSettings.TextFile);

            var res = FrequencyAnalyzer.Analyze(rawText.GetValueOrDefault())
                .Then(x => new TagsCloudLayouter(x).GetTextImages())
                .Then(x => Painter.Draw(x));

            return res;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG files (*.png)|*.png";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var res = image.Then(x => ImageSaver.SaveToFile(x, saveDialog.FileName, saveDialog.DefaultExt));

                if (!res.IsSuccess)
                {
                    ErrorMessageBox.ShowError(res.Error);
                }
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsManager.SettingsManager.SaveSettings(appSettings);
            Close();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorSelector = new ColorSelectorForm(appSettings.DrawingSettings.Colors, appSettings.DrawingSettings.BgColor);
            colorSelector.ShowDialog();
            if (colorSelector.DialogResult == DialogResult.OK)
            {
                appSettings.DrawingSettings.Colors = colorSelector.Colors;
                appSettings.DrawingSettings.BgColor = colorSelector.BGColor;
                SettingsManager.SettingsManager.SaveSettings(appSettings);
                RedrawImage(RebuildImage());
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var properties = new Properties(appSettings, providers);

            if (properties.ShowDialog() == DialogResult.OK)
            {
                appSettings = properties.appSettings;
                RedrawImage(RebuildImage());
            }
        }
    }
}
