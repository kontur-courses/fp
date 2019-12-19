using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TagCloud.Interfaces.GUI.Forms
{
    public class ApplicationSettingsForm : Form
    {
        private readonly ApplicationSettings settings;
        private readonly WordSelectorForm wordSelectorForm;

        private readonly Encoding[] encodings = {Encoding.GetEncoding(1251), Encoding.UTF8};

        public ApplicationSettingsForm(ApplicationSettings settings, WordSelectorForm wordSelectorForm)
        {
            this.settings = settings;
            this.wordSelectorForm = wordSelectorForm;
            InitializeForm();
        }

        private void InitializeForm()
        {
            Controls.Add(new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
            });

            var labelAnalyzer = new Label { Location = new Point(15, 15), Text = "Анализатор текста" };
            var wordSeletorBtn = new Button
            {
                Text = "Настройки выборки слов",
                Dock = DockStyle.Bottom
            };
            wordSeletorBtn.Click += (a,b) => wordSelectorForm.ShowDialog();

            var textAnalyzers = new ComboBox
            {
                Location = labelAnalyzer.Location + new Size(labelAnalyzer.Size.Width, 0)
            };

            var analyzerNames = settings.TextAnalyzers
                .Select(w => w.GetType().GetCustomAttribute<NameAttribute>()?.Name ?? "UnknownName")
                .ToArray();

            textAnalyzers.Items.AddRange(analyzerNames);

            var encodingsLabel = new Label
            {
                Text = "Кодировки",
                Location = labelAnalyzer.Location + new Size(0, labelAnalyzer.Size.Height + 15)
            };

            var encodingsBox = new ComboBox()
            {
                Location = labelAnalyzer.Location + new Size(encodingsLabel.Size.Width, labelAnalyzer.Size.Height)
            };
            var encodingNames = encodings.Select(e => e.BodyName).ToArray();
            encodingsBox.Items.AddRange(encodingNames);


            Controls.Add(labelAnalyzer);
            Controls.Add(textAnalyzers);
            Controls.Add(wordSeletorBtn);
            Controls.Add(encodingsLabel);
            Controls.Add(encodingsBox);

            textAnalyzers.SelectedIndexChanged += OnUpdateTextAnalyzer;
            encodingsBox.SelectedIndexChanged += OnUpdateEncoding;
        }

        private void OnUpdateEncoding(object sender, EventArgs e)
        {
            settings.TextEncoding = encodings[((ComboBox) sender).SelectedIndex];
        }

        private void OnUpdateTextAnalyzer(object sender, EventArgs e)
        {
            settings.CurrentTextAnalyzer = settings.TextAnalyzers[((ComboBox) sender).SelectedIndex];
        }
    }
}
