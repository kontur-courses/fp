using System;
using System.Drawing;
using TagCloudContainer.FileReaders;
using TagCloudContainer.FileSavers;
using TagCloudContainer.LayouterAlgorithms;
using TagCloudContainer.TaskResult;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagCloudContainer.UI
{
    public class Configuration
    {
        public int Coefficient { get; }
        public Result<Color> BackgroundColor { get; }
        public Result<Color> BrushColor { get; }
        public Result<IFileSaver> FileSaver { get; }
        public Result<IWordsPainter> Painter { get; }
        public Result<IFileReader> FileReader { get; }
        public Result<ICloudLayouterAlgorithm> Algorithm { get; }
        public Result<int> CanvasBorder { get; }
        public Result<int> CanvasHeight { get; }
        public Result<int> CanvasWidth { get; }
        public Result<double> AngleOffset { get; }
        public Result<double> RadiusOffset { get; }

        public Result<string> PathToSave { get; }

        public Configuration(IUi settings,
            WordsColoringFactory wordsColoringFactory,
            FileSaverFactory fileSaverFactory,
            FileReaderFactory fileReaderFactory,
            LayouterFactory layouterFactory)
        {
            AngleOffset = settings.AngleOffset > 1 || settings.AngleOffset < 0
                ? Result.OnFail<double>("Invalid angle offset")
                : Result.OnSuccess<double>(settings.AngleOffset);
            RadiusOffset = settings.RadiusOffset > 1 || settings.RadiusOffset < 0
                ? Result.OnFail<double>("Invalid radius offset")
                : Result.OnSuccess<double>(settings.AngleOffset);
            CanvasWidth = settings.CanvasWidth < settings.CanvasBorder * 2
                ? Result.OnFail<int>("Invalid canvas width")
                : Result.OnSuccess<int>(settings.CanvasWidth);
            CanvasHeight = settings.CanvasHeight < settings.CanvasBorder * 2
                ? Result.OnFail<int>("Invalid canvas height")
                : Result.OnSuccess<int>(settings.CanvasWidth);
            CanvasBorder = settings.CanvasBorder >= 0
                ? Result.OnSuccess(settings.CanvasBorder)
                : Result.OnFail<int>("Borders can't be less than zero");
            PathToSave = settings.PathToSave.Length > 0
                ? Result.OnSuccess(settings.PathToSave)
                : Result.OnFail<string>("Empty path to save");
            Coefficient = ScaleCoefficientCalculator.CalculateScaleCoefficient(settings.CanvasWidth, settings.CanvasHeight, settings.CanvasBorder);
            BackgroundColor = GetColorFromString(settings.BackGroundColor, "background");
            BrushColor = GetColorFromString(settings.BrushColor, "brush");
            FileSaver = fileSaverFactory.Create();
            Painter = wordsColoringFactory.Create();
            FileReader = fileReaderFactory.Create();
            Algorithm = layouterFactory.Create();
        }

        private static Result<Color> GetColorFromString(string color, string name)
        {
            var result = Color.FromName(color);
            return !result.IsKnownColor ? Result.OnFail<Color>($"Unknown {name} color") : Result.OnSuccess(result);
        }
    }
}