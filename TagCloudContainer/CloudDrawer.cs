using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.Result;
using TagCloudContainer.UI;

namespace TagCloudContainer
{
    public static class CloudDrawer
    {
        public static void DrawWords(Configuration configuration, IUi settings)
        {
            if (!CheckConfiguration(configuration).IsSuccess)
                return;
            var fileReader = configuration.FileReader.Value;
            var painter = configuration.Painter.Value;
            var canvas = new Bitmap(configuration.CanvasWidth.Value, configuration.CanvasHeight.Value);
            var graphics = Graphics.FromImage(canvas);
            graphics.Clear(configuration.BackgroundColor.Value);
            var frequencyDictionary =
                InputFileHandler.FormFrequencyDictionary(fileReader.FileToWordsArray(settings.PathToOpen).Value);
            if (!frequencyDictionary.IsSuccess)
            {
                Console.WriteLine(frequencyDictionary.Error);
                return;
            }

            var wordColorDictionary =
                painter.GetWordColorDictionary(frequencyDictionary.Value, configuration.BrushColor.Value);
            foreach (var pair in frequencyDictionary.Value)
            {
                var word = pair.Key;
                var wordCount = pair.Value;
                var rectangleHeight = wordCount * configuration.Coefficient.Value * word.Length +
                                      configuration.Coefficient.Value;
                var rectangleWidth = wordCount * 2 * configuration.Coefficient.Value;
                var rectangle =
                    configuration.Algorithm.Value.PutNextRectangle(new Size(rectangleHeight, rectangleWidth));

                var font = GetFontFromString(wordCount, settings.FontName, configuration.Coefficient.Value);
                if (!font.IsSuccess)
                {
                    Console.WriteLine(font.Error);
                    return;
                }

                graphics.DrawString(word, font.Value, new SolidBrush(wordColorDictionary[word]), rectangle.Value);
            }

            configuration.FileSaver.Value.SaveCanvas(configuration.PathToSave.Value, canvas);
            graphics.Dispose();
        }

        private static Result<bool> CheckConfiguration(Configuration configuration)
        {
            var configurationErrors = new List<string>
            {
                configuration.Coefficient.Error,
                configuration.BackgroundColor.Error,
                configuration.BrushColor.Error,
                configuration.FileSaver.Error,
                configuration.Painter.Error,
                configuration.FileReader.Error,
                configuration.Coefficient.Error,
                configuration.RadiusOffset.Error,
                configuration.AngleOffset.Error,
                configuration.CanvasBorder.Error,
                configuration.CanvasHeight.Error,
                configuration.CanvasWidth.Error,
                configuration.PathToSave.Error
            };
            var errors = configurationErrors.Where(error => error != null);
            if (!errors.Any())
                return new Result<bool>(null, true);
            foreach (var error in errors)
                Console.WriteLine(error);

            return new Result<bool>(errors.First());
        }

        private static Result<Font> GetFontFromString(int wordCount, string fontName, int coefficient)
        {
            var font = new Font(fontName, (coefficient + 2) * wordCount - 2);
            return font.Name != font.OriginalFontName
                ? new Result<Font>("Unknown font name")
                : new Result<Font>(null, font);
        }
    }
}