using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using ResultOf;
using TagCloudCreator;

namespace TagCloudGUIClient
{
    public partial class Form1 : Form
    {
        private CloudPrinter cloudPrinter;
        private List<IColorSelectorFabric> colorSelectors;
        private List<IBaseCloudLayouterFabric> layouters;


        public Form1(CloudPrinter cloudPrinter, IEnumerable<IColorSelectorFabric> colorSelectors,
            IEnumerable<IBaseCloudLayouterFabric> layouters)
        {
            Result.OfAction(InitializeComponent).Then(_ =>
                    fontSelector.Items.AddRange(FontFamily.Families.Select(x => x.Name).ToArray()))
                .Then(_ => this.colorSelectors = colorSelectors.ToList())
                .Then(_ => this.layouters = layouters.ToList())
                .Then(_ => layouterSelector.Items.AddRange(this.layouters.Select(x => x.Name).ToArray()))
                .Then(_ => colorSelectorSelector.Items.AddRange(this.colorSelectors.Select(x => x.Name).ToArray()))
                .Then(_ => this.cloudPrinter = cloudPrinter)
                .OnFail(x => MessageBox.Show(x));
        }

        private Size ImageSize { get; set; }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            Result.OfAction(RedrawImage).OnFail(x => MessageBox.Show("Draw Error"));
        }


        private void fileSelector_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"files with text|*.txt;*.doc;*.docx;*.html;*.pdf";
            if (openFileDialog.ShowDialog() == DialogResult.OK) textBox1.Text = openFileDialog.FileName;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Result.OfAction(() =>
            {
                using SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = @"png files (*.png)|*.png";
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                var stream = saveFileDialog.OpenFile();
                pictureBox1.Image.Save(stream, ImageFormat.Png);
                stream.Close();
            }).OnFail(x => MessageBox.Show(x));
        }

        private void RedrawImage()
        {
            if (!AllSelected())
                return;
            var fontFamily = FontFamily.Families[fontSelector.SelectedIndex];
            var colorSelector = Result
                .Of(() => colorSelectors.First(x => x.Name == (string) colorSelectorSelector.SelectedItem))
                .Then(x => x.Create());

            var path = textBox1.Text;
            colorSelector.Then(i =>
                    layouters.First(x => x.Name == (string) layouterSelector.SelectedItem)
                        .Create(new Point(ImageSize / 2)))
                .Then(layouter =>
                    cloudPrinter.DrawCloud(path,
                        layouter,
                        ImageSize,
                        fontFamily,
                        colorSelector.GetValueOrThrow()))
                .Then(bitmap => pictureBox1.Image = bitmap);
        }

        private bool AllSelected()
        {
            return fontSelector.SelectedIndex != -1
                   && layouterSelector.SelectedIndex != -1
                   && colorSelectorSelector.SelectedIndex != -1
                   && textBox1.Text != "";
        }

        private void sizeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pair = ((string) sizeSelector.SelectedItem).Split('x');
            ImageSize = new Size(int.Parse(pair[0]), int.Parse(pair[1]));
        }
    }
}