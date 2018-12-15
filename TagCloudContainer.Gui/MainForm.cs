using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using TagsCloudContainer;
using TagsCloudContainer.WordsReaders;

namespace TagCloudContainer.Gui
{
    public partial class MainForm : Form
    {
        private IContainer container;
        private Color chooseColor = Color.BlueViolet;

        public MainForm()
        {
            InitializeComponent();
            container = new CloudContainerBuilder().BuildTagsCloudContainer();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            resultPictureBox.BorderStyle = BorderStyle.FixedSingle;
            resultPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    container.Resolve<IWordsReader>()
                        .AsResult()
                        .Then(reader => string.Join(Environment.NewLine,
                            reader.GetWords(openFileDialog.FileName)))
                        .Then(words => wordsTextBox.Text = words)
                        .OnFail(err => statusLabel.Text = err);
                }
            }
        }

        private void SaveResultButton_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog {Filter = "Images (*.png)|*.png"})
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    resultPictureBox.Image.Save(saveFileDialog.FileName, ImageFormat.Png);
                }
            }
        }

        private void ChooseFontColor_Click(object sender, EventArgs e)
        {
            var colorPickerDialog = new ColorDialog {Color = chooseColor};
            colorPickerDialog.ShowDialog();
            chooseColor = colorPickerDialog.Color;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Готов к работе.";

            var size = new Size(int.Parse(resultWidthTextBox.Text), int.Parse(resultHeightTextBox.Text));
            var font = new Font(FontFamily.GenericMonospace, 12);

            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<Config>()
                    .AsResult()
                    .Then(config => FillConfig(config, font, size))
                    .Then(c => scope.Resolve<TagsCloudBuilder>()
                        .Visualize(wordsTextBox.Lines))
                    .Then(image => resultPictureBox.Image = image)
                    .OnFail(err => statusLabel.Text = err);
            }
        }

        private void FillConfig(Config config, Font font, Size size)
        {
            config.Color = chooseColor;
            config.Font = font;
            config.ImageSize = size;
        }
    }
}