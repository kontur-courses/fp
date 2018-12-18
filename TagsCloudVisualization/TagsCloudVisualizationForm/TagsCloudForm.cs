using System;
using System.Windows.Forms;
using TagsCloudVisualization;
using Autofac;

namespace TagsCloudVisualizationForm
{
    public partial class TagsCloudForm : Form
    {
        private string inputFilePath;
        private readonly TagsCloudApp tagsCloudApp;

        public TagsCloudForm()
        {
            InitializeComponent();
            tagsCloudApp = TagsCloudVisualizationContainerConfig.GetContainer().Resolve<TagsCloudApp>();
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            GetOptions()
                .Then(tagsCloudApp.Run)
                .OnFail(error => MessageBox.Show(error, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error));
        }

        private Result<Options> GetOptions()
        {
            return new Options
            {
                Color = colorTextBox.Text,
                PointGenerator = formPointGeneratorComboBox.SelectedItem?.ToString() ?? formPointGeneratorComboBox.Text,
                FilePath = inputFilePath,
                FontName = fontNameTextBox.Text,
                ImageSize = $"{imageSizeTextBox1.Text}x{imageSizeTextBox2.Text}",
                OutFormat = outFormatComboBox.SelectedItem?.ToString() ?? formPointGeneratorComboBox.Text
            };
        }

        private void exitBtn_Click(object sender, EventArgs e) => Close();

        private void inputFilePathBtn_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = @"txt files (*.txt)|*.txt|doc files (*.doc;*.docx)|*.doc;*docx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    inputFilePath = openFileDialog.FileName;
            }
        }
    }
}
