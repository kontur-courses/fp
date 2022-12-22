using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.FileReaders;
using TagCloudContainer.FileSavers;
using TagCloudContainer.LayouterAlgorithms;
using TagCloudContainer.Result;
using TagCloudContainer.UI;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagCloudContainer
{
    public static class CircularCloudDrawer
    {
        public static void DrawWords(
            WordsColoringFactory wordsColoringFactory,
            FileSaverFactory fileSaverFactory,
            FileReaderFactory fileReaderFactory,
            IUi settings,
            LayouterFactory layouterFactory)
        {
            var painter = wordsColoringFactory.Create();
            var fileSaver = fileSaverFactory.Create();
            var fileReader = fileReaderFactory.Create();
            var layouter = layouterFactory.Create(new Spiral(settings));
            if (!CheckResultSuccess(painter) ||
                !CheckResultSuccess(fileSaver) ||
                !CheckResultSuccess(fileReader) ||
                !CheckResultSuccess(layouter))
                return;
            var canvas = new Bitmap(settings.CanvasWidth, settings.CanvasHeight);
            var graphics = Graphics.FromImage(canvas);
            var color = GetColorFromString(settings.BackGroundColor);
            var brushColor = GetColorFromString(settings.BrushColor);
            var coefficient = ScaleCoefficientCalculator.CalculateScaleCoefficient(settings.CanvasWidth,
                settings.CanvasHeight, settings.CanvasBorder);
            if (!CheckResultSuccess(color) || !CheckResultSuccess(coefficient) || !CheckResultSuccess(brushColor))
                return;
            graphics.Clear(color.Value);

            var frequencyDictionary =
                InputFileHandler.FormFrequencyDictionary(fileReader.Value.FileToWordsArray(settings.PathToOpen),
                    settings);
            var brushColors = painter.Value.GetColorsSequence(frequencyDictionary, brushColor.Value);
            if (!CheckResultSuccess(brushColors))
                return;
            var counter = 0;
            foreach (var pair in frequencyDictionary)
            {
                DrawWord(pair, coefficient.Value, layouter.Value, settings, graphics, brushColors.Value[counter]);
                counter++;
            }

            fileSaver.Value.SaveCanvas(settings.PathToSave, canvas);
            graphics.Dispose();
        }

        private static void DrawWord(KeyValuePair<string, int> pair, int coefficient, ICloudLayouterAlgorithm layouter,
            IUi settings, Graphics graphics, Color brushColor)
        {
            var word = pair.Key;
            var wordCount = pair.Value;
            var rectangleHeight = wordCount * coefficient * word.Length + coefficient;
            var rectangleWidth = wordCount * 2 * coefficient;
            var location = layouter.PlaceNextWord(word, wordCount, coefficient);
            var font = GetFontFromString(coefficient, settings.FontName, wordCount);
            if (!CheckResultSuccess(location) || !CheckResultSuccess(font))
                return;
            var rectangle = new Rectangle(location.Value, new Size(rectangleHeight, rectangleWidth));
            graphics.DrawString(word, font.Value, new SolidBrush(brushColor), rectangle);
        }

        private static Result<Color> GetColorFromString(string color)
        {
            var result = Color.FromName(color);
            return !result.IsKnownColor ? new Result<Color>("Unknown color") : new Result<Color>(null, result);
        }

        private static Result<Font> GetFontFromString(int coefficient, string fontName, int wordCount)
        {
            var font = new Font(fontName, (coefficient + 2) * wordCount - 2);
            return font.Name != font.OriginalFontName
                ? new Result<Font>("Unknown font name")
                : new Result<Font>(null, font);
        }

        private static bool CheckResultSuccess<T>(Result<T> result)
        {
            if (result.IsSuccess) return true;
            Console.WriteLine(result.Error);
            return false;
        }
    }
}