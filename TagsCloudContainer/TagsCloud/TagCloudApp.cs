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
        private string _fontName;
        private readonly FontSizeCalculator _fontSizeCalculator = new FontSizeCalculator();
        private string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");

        private const double DefaultAngleStep = 0.02;
        private const double DefaultRadiusStep = 0.04;
        private const int HalfWidthHeight = 2;

        public string FontName { get; set; }

        public TagCloudApp(IPreprocessor preprocessor, IImageSettings imageSettings, FileReader fileReader)
        {
            _preprocessor = preprocessor;
            _imageSettings = imageSettings;
            _fileReader = fileReader;
        }

        public void Run(CommandLineOptions options)
        {
            var result = _fileReader.ReadFile(options.TextFilePath);

            if (result.IsSuccess)
            {
                var words = result.Value;
                SetFontAndImageSettings(options);

                var processedWords = _preprocessor.Process(words, options.BoringWordsFilePath);
                var uniqueWordCount = CountUniqueWords(processedWords);

                var (fontColor, highlightColor) = GetColors(options.FontColor, options.HighlightColor);

                var tagCloudImageResult = GenerateTagCloud(processedWords, options.FontName, fontColor, highlightColor, options.PercentageToHighLight);

                if (tagCloudImageResult.IsSuccess)
                {
                    var tagCloudImage = tagCloudImageResult.Value;
                    SaveTagCloudImage(tagCloudImage, outputDirectory, uniqueWordCount);
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

        private void SetFontAndImageSettings(CommandLineOptions options)
        {
            _fontName = options.FontName;
            _imageSettings.UpdateImageSettings(options.ImageWidth, options.ImageHeight);
        }

        public Result<None> SaveTagCloudImage(Bitmap tagCloudImage, string outputDirectory, int uniqueWordCount)
        {
            try
            {
                var outputFileName = $"{uniqueWordCount}-tagsCloud.png";
                var outputPath = Path.Combine(outputDirectory, outputFileName);
                tagCloudImage.Save(outputPath);

                return Result.Ok().Then(_ =>
                {
                    Console.WriteLine($"Tag cloud image saved to {outputPath}. Original word count: {uniqueWordCount}");
                });
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

                    var sortedWords = SortWordsByFrequency(wordFrequencies);

                    var fontSizes = CalculateAndPutRectangles(layouter, sortedWords, uniqueWords, wordFrequencies, mostPopularWord, fontName);

                    var rectangles = layouter.Rectangles.ToList();

                    return Result.Ok(Visualizer.VisualizeRectangles(rectangles, uniqueWords, _imageSettings.Width, _imageSettings.Height,
                        fontSizes, FontName, fontColor, highlightColor, percentageToHighlight, wordFrequencies: wordFrequencies));
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
            string mostPopularWord = null;
            int maxFrequency = int.MinValue;

            foreach (var pair in wordFrequencies)
            {
                if (pair.Value > maxFrequency)
                {
                    mostPopularWord = pair.Key;
                    maxFrequency = pair.Value;
                }
            }

            return mostPopularWord;
        }

        private IEnumerable<string> SortWordsByFrequency(Dictionary<string, int> wordFrequencies)
        {
            return wordFrequencies.Keys.OrderByDescending(word => wordFrequencies[word]);
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
            return new Point(width / HalfWidthHeight, height / HalfWidthHeight);
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
