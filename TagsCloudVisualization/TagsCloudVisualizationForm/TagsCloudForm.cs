using System;
using System.IO;
using System.Windows.Forms;
using TagsCloudVisualization;
using Autofac;

namespace TagsCloudVisualizationForm
{
    public partial class TagsCloudForm : Form
    {
        private string inputFilePath;

        public TagsCloudForm()
        {
            InitializeComponent();
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            var options = new Options
            {
                Color = colorTextBox.Text,
                PointGenerator = formPointGeneratorComboBox.SelectedItem?.ToString() ?? formPointGeneratorComboBox.Text,
                FilePath = inputFilePath,
                FontName = fontNameTextBox.Text,
                ImageSize = $"{imageSizeTextBox1.Text}x{imageSizeTextBox2.Text}",
                OutFormat = outFormatComboBox.SelectedItem?.ToString() ?? formPointGeneratorComboBox.Text
            };
            var container = TagsCloudVisualizationContainerConfig.GetCompositionRoot();
            container.Resolve<TagsCloudApp>().Run(options, container);
        }

        private void exitBtn_Click(object sender, EventArgs e) => Close();

        private void inputFilePathBtn_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = @"txt files (*.txt)|*.txt|doc files (*.doc;*.docx)|*.doc;*docx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    inputFilePath = openFileDialog.SafeFileName;
            }
        }
    }
}
