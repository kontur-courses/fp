using System.Drawing;
using TagCloudContainer.FileReaders;
using TagCloudContainer.FileSavers;
using TagCloudContainer.LayouterAlgorithms;
using TagCloudContainer.Result;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagCloudContainer.UI
{
    public class Configuration
    {
        public Result<int> Coefficient { get; }
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
                ? new Result<double>("Invalid angle offset")
                : new Result<double>(null, settings.AngleOffset);
            RadiusOffset = settings.RadiusOffset > 1 || settings.RadiusOffset < 0
                ? new Result<double>("Invalid radius offset")
                : new Result<double>(null, settings.RadiusOffset);
            CanvasWidth = settings.CanvasWidth < settings.CanvasBorder * 2
                ? new Result<int>("Too small canvas width")
                : new Result<int>(null, settings.CanvasWidth);
            CanvasHeight = settings.CanvasHeight < settings.CanvasBorder * 2
                ? new Result<int>("Too small canvas height")
                : new Result<int>(null, settings.CanvasHeight);
            CanvasBorder = settings.CanvasBorder > 0
                ? new Result<int>(null, settings.CanvasBorder)
                : new Result<int>("Borders can't be less than zero");
            PathToSave = settings.PathToSave.Length > 0
                ? new Result<string>(null, settings.PathToSave)
                : new Result<string>("Empty path to save");
            Coefficient = ScaleCoefficientCalculator.CalculateScaleCoefficient(settings.CanvasWidth,
                settings.CanvasHeight, settings.CanvasBorder);
            BackgroundColor = GetColorFromString(settings.BackGroundColor);
            BrushColor = GetColorFromString(settings.BrushColor);
            FileSaver = fileSaverFactory.Create();
            Painter = wordsColoringFactory.Create();
            FileReader = fileReaderFactory.Create();
            Algorithm = layouterFactory.Create();
        }

        private static Result<Color> GetColorFromString(string color)
        {
            var result = Color.FromName(color);
            return !result.IsKnownColor ? new Result<Color>("Unknown color") : new Result<Color>(null, result);
        }
    }
}