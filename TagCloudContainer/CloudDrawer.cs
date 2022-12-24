using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.TaskResult;
using TagCloudContainer.UI;

namespace TagCloudContainer
{
    public static class CloudDrawer
    {
        public static void DrawWords(Configuration configuration, IUi settings)
        {
            if (!CheckConfiguration(configuration))
                return;
            var fileReader = configuration.FileReader.Value;
            var painter = configuration.Painter.Value;
            var canvas = new Bitmap(configuration.CanvasWidth.Value, configuration.CanvasHeight.Value);
            var graphics = Graphics.FromImage(canvas);
            graphics.Clear(configuration.BackgroundColor.Value);
            var frequencyDictionary =
                InputFileHandler.FormFrequencyDictionary(fileReader.FileToWordsArray(settings.PathToOpen).Value);
            Dictionary<string, Color> wordColorDictionary = null;
            frequencyDictionary.Then(dictionary =>
                wordColorDictionary = painter.GetWordColorDictionary(dictionary, configuration.BrushColor.Value));
            if (wordColorDictionary == null)
                return;
            foreach (var pair in frequencyDictionary.Value)
            {
                var word = pair.Key;
                var wordCount = pair.Value;
                var rectangleHeight = wordCount * configuration.Coefficient * word.Length +
                                      configuration.Coefficient;
                var rectangleWidth = wordCount * 2 * configuration.Coefficient;
                var rectangle =
                    configuration.Algorithm.Value.PutNextRectangle(new Size(rectangleHeight, rectangleWidth));

                var font = GetFontFromString(wordCount, settings.FontName, configuration.Coefficient);
                font.Then(f =>
                    graphics.DrawString(word, f, new SolidBrush(wordColorDictionary[word]), rectangle.Value));
                if (font.Error != null)
                    return;
            }

            configuration.FileSaver.Value.SaveCanvas(configuration.PathToSave.Value, canvas);
            graphics.Dispose();
        }

        private static bool CheckConfiguration(Configuration configuration)
        {
            var configurationErrors = new List<string>
            {
                configuration.BackgroundColor.Error,
                configuration.BrushColor.Error,
                configuration.FileSaver.Error,
                configuration.Painter.Error,
                configuration.FileReader.Error,
                configuration.RadiusOffset.Error,
                configuration.AngleOffset.Error,
                configuration.CanvasBorder.Error,
                configuration.CanvasHeight.Error,
                configuration.CanvasWidth.Error,
                configuration.PathToSave.Error,
                configuration.Algorithm.Error
            };
            var errors = configurationErrors.Distinct().Where(error => error != null).ToArray();
            if (errors.Length == 0)
                return true;
            foreach (var error in errors)
                Console.WriteLine(error);
            return false;
        }

        private static Result<Font> GetFontFromString(int wordCount, string fontName, int coefficient)
        {
            var font = new Font(fontName, (coefficient + 2) * wordCount - 2);
            return font.Name != font.OriginalFontName
                ? Result.OnFail<Font>("Unknown font name")
                : Result.OnSuccess(font);
        }
    }
}