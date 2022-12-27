using MystemHandler;
using TagCloudContainer;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Result;
using TagCloudGUI.Interfaces;
using TagCloudGUI.Settings;

namespace TagCloudGUI.Actions
{
    public class TagAction : IActionForm
    {
        private readonly IImageSettingsProvider image;
        private readonly IPresetsSettings presetsSettings;
        private readonly IPointProvider pointFigure;
        private readonly IRectangleBuilder rectangleBuilder;
        private readonly IAlgorithmSettings algorithmSettings;
        private readonly IBoringWordsFilter boringWordsFilter;
        private readonly Palette palette;

        public TagAction(
            IPointProvider pointFigure,
            IRectangleBuilder rectangleBuilder,
            IPresetsSettings presetsSettings,
            IImageSettingsProvider image,
            IAlgorithmSettings algorithmSettings,
            IBoringWordsFilter boringWordsFilter,
            Palette palette)
        {
            this.image = image;
            this.algorithmSettings = algorithmSettings;
            this.palette = palette;
            this.presetsSettings = presetsSettings;
            this.rectangleBuilder = rectangleBuilder;
            this.pointFigure = pointFigure;
            this.boringWordsFilter = boringWordsFilter;
        }

        string IActionForm.Category => "Рисование";

        string IActionForm.Name => "Нарисовать";

        string IActionForm.Description => "Нарисовать облако тегов";

        void IActionForm.Perform()
        {
            var filePath = GetFilePathDialog();

            if (!filePath.IsSuccess)
            {
                MessageBox.Show(filePath.Error);
                return;
            }

            algorithmSettings.FilePath = filePath.Value;
            SettingsForm.For(algorithmSettings).ShowDialog();

            pointFigure.Reset();

            var cloud = new TagCloud();
            var tagsResult = InitialTags(algorithmSettings.FilePath);

            if (!tagsResult.IsSuccess)
            {
                MessageBox.Show(tagsResult.Error);
                return;
            }

            var res = cloud.CreateTagCloud(
                pointFigure,
                rectangleBuilder,
                tagsResult.Value);

            if (!res.IsSuccess)
            {
                MessageBox.Show(res.Error);
                return;
            }

            var size = ImageSizer.GetImageSize(cloud.GetRectangles());
            image.RecreateImage(new ImageSettings { Height = size.Height, Width = size.Width });
            
            DrawCloud(cloud);
        }

        private Result<string> GetFilePathDialog()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = "Txt files (*.txt)|*.txt|Doc files (*.doc)|*.doc|Docx files (*.docx)|*.docx";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            var result = openFileDialog1.ShowDialog();

            if (result == DialogResult.Cancel || openFileDialog1.FileName is null or "")
                return Result.Fail<string>("Файл не был выбран");

            return Result.Ok(openFileDialog1.FileName);
        }

        private void DrawCloud(TagCloud cloud)
        {
            presetsSettings.Drawer.DrawCloudFromPalette(cloud.GetRectangles(), image, palette);
        }

        public Result<IEnumerable<ITag>> InitialTags(string filePath)
        {
            
            var originalTextResult = presetsSettings.Reader.ReadFile(filePath);

            if (!originalTextResult.IsSuccess)
                return Result.Fail<IEnumerable<ITag>>("Ошибка при чтении файла: " + originalTextResult.Error);

            var parsedTextResult = presetsSettings.Parser.Parse(originalTextResult.Value);

            if (!parsedTextResult.IsSuccess)
                return Result.Fail<IEnumerable<ITag>>("Ошибка при парсинге файла: " + parsedTextResult.Error);

            if (presetsSettings.Filtered == Switcher.Enabled)
            {
                parsedTextResult = boringWordsFilter.FilterWords(parsedTextResult.Value);

                if (!parsedTextResult.IsSuccess)
                    return Result.Fail<IEnumerable<ITag>>("Ошибка при исключении скучных слов: " + parsedTextResult.Error);
            }

            var formattedTagsResult = presetsSettings.ToLowerCase == Switcher.Enabled
                ? presetsSettings.Formatter.Normalize(parsedTextResult.Value, x => x.ToLower())
                : parsedTextResult;

            if (!formattedTagsResult.IsSuccess)
                return Result.Fail<IEnumerable<ITag>>("Ошибка при нормализации текста: " + formattedTagsResult.Error);

            var freqTagsResult = presetsSettings.FrequencyCounter.GetTagsFrequency(formattedTagsResult.Value);

            if (!freqTagsResult.IsSuccess)
                return Result.Fail<IEnumerable<ITag>>("Ошибка при подсчете частот слов: " + freqTagsResult.Error);

            return presetsSettings.FontSizer.GetTagsWithSize(freqTagsResult.Value, algorithmSettings.FontSettings);
        }
    }
}
