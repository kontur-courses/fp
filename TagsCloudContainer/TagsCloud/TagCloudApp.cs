using System.Drawing;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Utility;

namespace TagsCloudContainer.TagsCloud
{
    public class TagCloudApp
    {
        private readonly IPreprocessor _preprocessor;
        private readonly IImageSettings _imageSettings;
        private readonly FileReader _fileReader;
        private readonly FontSizeCalculator _fontSizeCalculator = new FontSizeCalculator();

        private string _fontName;
        private string outputDirectory = @"..\..\..\output";
        private const double DefaultAngleStep = 0.02;
        private const double DefaultRadiusStep = 0.04;
        private const int Half = 2;

        public TagCloudApp(IPreprocessor preprocessor, IImageSettings imageSettings, FileReader fileReader)
        {
            _preprocessor = preprocessor;
            _imageSettings = imageSettings;
            _fileReader = fileReader;
        }


        public void SetFontName(string fontName)
        {
            _fontName = fontName;
        }

        public void Run(CommandLineOptions options)
        {
            var result = _fileReader.ReadFile(options.TextFilePath);

            if (result.IsSuccess)
            {
                var words = result.Value;
                SetFontAndImageSettings(options.FontName, options.ImageWidth, options.ImageHeight);

                var processedWords = _preprocessor.Process(words, options.BoringWordsFilePath);
                var uniqueWordCount = CountUniqueWords(processedWords);

                var (fontColor, highlightColor) = GetColors(options.FontColor, options.HighlightColor);

                var tagCloudImageResult = GenerateTagCloud(processedWords, options.FontName, fontColor, highlightColor, options.PercentageToHighLight);

                if (tagCloudImageResult.IsSuccess)
                {
                    var tagCloudImage = tagCloudImageResult.Value;
                    SaveTagCloudImage(tagCloudImage, outputDirectory, uniqueWordCount).GetValueOrThrow();
                }
                else
                {
                    Console.WriteLine($"Error generating tag cloud image: {tagCloudImageResult.Error}");
                }
            }
            else
            {
                Console.WriteLine($"Error reading file: {result.Error}");
            }
        }

        private (Color fontColor, Color highlightColor) GetColors(string fontColorName, string highlightColorName)
        {
            return (Color.FromName(fontColorName), Color.FromName(highlightColorName));
        }

        private void SetFontAndImageSettings(string fontName, int imageWidth, int imageHeight)
        {
            SetFontName(fontName);
            _imageSettings.UpdateImageSettings(imageWidth, imageHeight);
        }

        public Result<None> SaveTagCloudImage(Bitmap tagCloudImage, string outputDirectory, int uniqueWordCount)
        {
            try
            {
                var outputFileName = $"{uniqueWordCount}-tagsCloud.png";
                var outputPath = Path.Combine(outputDirectory, outputFileName);
                tagCloudImage.Save(outputPath);

                Console.WriteLine($"Tag cloud image saved to {outputPath}. Original word count: {uniqueWordCount}");

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail<None>($"Error saving tag cloud image: {ex.Message}");
            }
        }

        private int CountUniqueWords(IEnumerable<string> words)
        {
            var uniqueWords = new HashSet<string>(words, StringComparer.OrdinalIgnoreCase);
            return uniqueWords.Count;
        }

        private Result<Bitmap> GenerateTagCloud(IEnumerable<string> words, string fontName, Color fontColor, Color highlightColor, double percentageToHighlight)
        {
            try
            {
                var layouterResult = CreateLayouter();

                if (layouterResult.IsSuccess)
                {
                    var layouter = layouterResult.Value;
                    var uniqueWords = new HashSet<string>();
                    var wordFrequencies = _fontSizeCalculator.CalculateWordFrequencies(words);

                    var mostPopularWord = GetMostPopularWord(wordFrequencies);

                    var sortedWords = SortWordsByFrequency(words, wordFrequencies);

                    var fontSizes = CalculateAndPutRectangles(layouter, sortedWords, uniqueWords, wordFrequencies, mostPopularWord, fontName);

                    var rectangles = layouter.Rectangles.ToList();

                    return Result.Ok(Visualizer.VisualizeRectangles(rectangles, uniqueWords, _imageSettings.Width, _imageSettings.Height,
                        fontSizes, _fontName, fontColor, highlightColor, percentageToHighlight, wordFrequencies: wordFrequencies));
                }
                else
                {
                    return Result.Fail<Bitmap>($"Error generating tag cloud: {layouterResult.Error}");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Bitmap>($"Error generating tag cloud: {ex.Message}");
            }
        }

        private string GetMostPopularWord(Dictionary<string, int> wordFrequencies)
        {
            return wordFrequencies.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;
        }

        private IEnumerable<string> SortWordsByFrequency(IEnumerable<string> words, Dictionary<string, int> wordFrequencies)
        {
            return words.OrderByDescending(word => wordFrequencies[word]);
        }

        private List<int> CalculateAndPutRectangles(CircularCloudLayouter layouter, IEnumerable<string> sortedWords, HashSet<string> uniqueWords, Dictionary<string, int> wordFrequencies, string mostPopularWord, string fontName)
        {
            var fontSizes = new List<int>();

            foreach (var word in sortedWords)
            {
                if (uniqueWords.Add(word))
                {
                    var fontSize = _fontSizeCalculator.CalculateWordFontSize(word, wordFrequencies);
                    fontSizes.Add(fontSize);

                    var font = new Font(fontName, fontSize);
                    layouter.PutNextRectangle(word, font);
                }
            }

            return fontSizes;
        }

        public static Point CalculateCenter(int width, int height)
        {
            return new Point(width / Half, height / Half);
        }

        private Result<CircularCloudLayouter> CreateLayouter(double angleStep = DefaultAngleStep, double radiusStep = DefaultRadiusStep)
        {
            try
            {
                var center = CalculateCenter(_imageSettings.Width, _imageSettings.Height);

                var spiralResult = Spiral.Create(center, angleStep, radiusStep);

                return spiralResult.IsSuccess
                    ? Result.Ok(new CircularCloudLayouter(center, spiralResult.Value))
                    : Result.Fail<CircularCloudLayouter>($"Error creating CircularCloudLayouter: {spiralResult.Error}");
            }
            catch (Exception ex)
            {
                return Result.Fail<CircularCloudLayouter>($"Error creating CircularCloudLayouter: {ex.Message}");
            }
        }
    }
}
