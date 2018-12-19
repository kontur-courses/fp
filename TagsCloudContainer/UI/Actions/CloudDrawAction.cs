using System.IO;
using System.Linq;
using System.Windows.Forms;
using ResultOf;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.Painter;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.UI.Actions
{
    public class CloudDrawAction : IUiAction
    {
        private readonly LayouterApplicator applicator;
        private readonly IFilePathProvider filePath;
        private readonly FrequencyCounter frequencyCounter;
        private readonly PictureBoxImageHolder imageHolder;
        private readonly TagCloudPainter painter;
        private readonly IWordsPreprocessor[] preprocessors;
        private readonly WordsPreprocessorSettings preprocessorSettings;
        private readonly IFileReader reader;

        public CloudDrawAction(IFileReader reader,
            IWordsPreprocessor[] preprocessors,
            FrequencyCounter frequencyCounter,
            WordsPreprocessorSettings preprocessorSettings,
            IFilePathProvider filePath,
            TagCloudPainter painter,
            LayouterApplicator applicator,
            PictureBoxImageHolder imageHolder)
        {
            this.reader = reader;
            this.preprocessors = preprocessors;
            this.frequencyCounter = frequencyCounter;
            this.preprocessorSettings = preprocessorSettings;
            this.filePath = filePath;
            this.painter = painter;
            this.applicator = applicator;
            this.imageHolder = imageHolder;
        }

        public MenuCategory Category => MenuCategory.TagCloud;
        public string Name => "Cгенерировать облако";
        public string Description => "Сгенерировать облако из файла";

        public void Perform()
        {
            ShowOpenFileDialog<string>()
                .Then(s => reader.Read())
                .Then(ShowSettingsDialog)
                .Then(words => preprocessors.Aggregate(words,
                    (current, wordsPreprocessor) => wordsPreprocessor.Process(current).GetValueOrThrow()))
                .Then(frequencyCounter.CountWordFrequencies)
                .Then(words => applicator.GetWordsAndRectangles(words.ToArray()))
                .Then(wordInfos => painter.Paint(applicator.WordsCenter, wordInfos))
                .Then(none => imageHolder.UpdateUi())
                .OnFail(error =>
                {
                    if (error != "")
                        MessageBox.Show(error);
                });
        }

        private Result<T> ShowSettingsDialog<T>(T input)
        {
            var res = SettingsForm.For(preprocessorSettings).ShowDialog();
            return res != DialogResult.OK
                ? Result.Fail<T>("")
                : Result.Ok(input);
        }

        private Result<T> ShowOpenFileDialog<T>(T input = default(T))
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetFullPath(filePath.WordsFilePath),
                DefaultExt = "txt",
                Filter = "Файлы (*.txt)|*.txt"
            };
            var res = dialog.ShowDialog();
            if (res != DialogResult.OK)
                return Result.Fail<T>("");
            filePath.WordsFilePath = dialog.FileName;
            return Result.Ok(input);
        }
    }
}