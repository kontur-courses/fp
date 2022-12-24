using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Autofac;
using TagsCloudContainer;

namespace TagsCloudGUI
{
    public partial class MainForm : Form
    {
        private string fileText;
        private readonly IDrawer drawer;
        private readonly IInputTextProvider inputTextProvider;
        private readonly DefaultDrawerSettingsProvider defaultDrawerSettingsProvider;
        private readonly CircularCloudSettingsProvider circularCloudSettingsProvider;

        public MainForm(IDrawer drawer, IInputTextProvider inputTextProvider,
            DefaultDrawerSettingsProvider defaultDrawerSettingsProvider,
            CircularCloudSettingsProvider circularCloudSettingsProvider)
        {
            InitializeComponent();
            this.drawer = drawer;
            this.inputTextProvider = inputTextProvider;
            this.defaultDrawerSettingsProvider = defaultDrawerSettingsProvider;
            this.circularCloudSettingsProvider = circularCloudSettingsProvider;
        }

        private void but_fileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog.FileName;
            fileText = File.ReadAllText(filename);
        }

        private void but_generate_Click(object sender, EventArgs e)
        {
            try
            {
                var parsedResult = TryToParse(box_stepSize.Text);
                if (!parsedResult.IsSuccess)
                {
                    ShowErrorMessage(parsedResult);
                    return;
                }
                circularCloudSettingsProvider.CircularCloudSettings.StepSize = parsedResult.Value;
                circularCloudSettingsProvider.CircularCloudSettings.StepSize =
                    circularCloudSettingsProvider.CircularCloudSettings.StepSize <= 0
                        ? 1
                        : circularCloudSettingsProvider.CircularCloudSettings.StepSize;
                var bitmapResult = drawer.DrawImage(fileText);
                if(bitmapResult.IsSuccess)
                    pic_main.BackgroundImage = bitmapResult.Value;
                else
                    ShowErrorMessage(bitmapResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void but_save_Click(object sender, EventArgs e)
        {
            var heigthResult = TryToParse(box_heightPic.Text);
            var widthResult = TryToParse(box_widthPic.Text);
            if (!heigthResult.IsSuccess)
            {
                ShowErrorMessage(heigthResult);
                return;
            }
            if (!widthResult.IsSuccess)
            {
                ShowErrorMessage(widthResult);
                return;
            }
            var heigth = heigthResult.Value;
            var width = widthResult.Value;
            var image = pic_main.BackgroundImage;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG picture(*.png)|*.png|JPG picture(*.jpg)|*.jpg";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog.FileName;
            if (heigth > 0 && width > 0)
            {
                image = new Bitmap(image, new Size(width, heigth));
            }

            image.Save(filename);
        }

        private void but_backgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.Color = defaultDrawerSettingsProvider.DefaultDrawerSettings.BackgroundColor;
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            defaultDrawerSettingsProvider.DefaultDrawerSettings.BackgroundColor = colorDialog.Color;
        }

        private void but_fontColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.Color = defaultDrawerSettingsProvider.DefaultDrawerSettings.FontColor;
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            defaultDrawerSettingsProvider.DefaultDrawerSettings.FontColor = colorDialog.Color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = defaultDrawerSettingsProvider.DefaultDrawerSettings.Font;
            if (fontDialog.ShowDialog() == DialogResult.Cancel)
                return;
            Font font = fontDialog.Font;
            defaultDrawerSettingsProvider.DefaultDrawerSettings.Font = font;
        }

        private Result<int> TryToParse(string str)
        {
            return Result.Of(() => int.Parse(str), "Введенная строка не является числом");
        }

        private void but_openExludedWordForm_Click(object sender, EventArgs e)
        {
            var form = new ExludedWordsForm(inputTextProvider.WordsFilter);
            form.Show();
        }

        private void ShowErrorMessage<T>(Result<T> result)
        {
            if (!result.IsSuccess)
                MessageBox.Show(result.Error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}