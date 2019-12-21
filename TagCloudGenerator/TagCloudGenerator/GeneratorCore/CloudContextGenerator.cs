using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagCloudGenerator.Clients;
using TagCloudGenerator.Clients.VocabularyParsers;
using TagCloudGenerator.GeneratorCore.CloudLayouters;
using TagCloudGenerator.GeneratorCore.TagClouds;
using TagCloudGenerator.GeneratorCore.Tags;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.GeneratorCore
{
    public class CloudContextGenerator : ICloudContextGenerator
    {
        private const string WidthGroupName = "width";
        private const string HeightGroupName = "height";

        private static readonly Regex imageSizeValidator =
            new Regex($@"(?<{WidthGroupName}>\d{{3,4}})x(?<{HeightGroupName}>\d{{3,4}})");

        private static readonly Regex hexColorPattern = new Regex(@"#[0-9a-fA-F]{8}");

        private readonly ITagCloudOptions<ITagCloud> cloudOptions;
        private readonly ICloudVocabularyParser vocabularyParser;

        public CloudContextGenerator(ITagCloudOptions<ITagCloud> cloudOptions,
                                     ICloudVocabularyParser vocabularyParser)
        {
            this.cloudOptions = cloudOptions;
            this.vocabularyParser = vocabularyParser;
        }

        public Result<TagCloudContext> GetTagCloudContext()
        {
            var imageFilename = VerifyFilename(cloudOptions.ImageFilename);
            var cloudVocabulary = ParseCloudVocabulary(cloudOptions.CloudVocabularyFilename);
            var excludedWords = ParseCloudVocabulary(cloudOptions.ExcludedWordsVocabularyFilename);
            var imageSize = ParseImageSize(cloudOptions.ImageSize);
            var backgroundColor = ParseColor(cloudOptions.BackgroundColor);
            var tagStyleByTagType = TagStylesParse(
                cloudOptions.GroupsCount, cloudOptions.MutualFont, cloudOptions.FontSizes, cloudOptions.TagColors);

            var foundErrorResult = Result.FindErrorResult(imageFilename,
                                                          cloudVocabulary,
                                                          excludedWords,
                                                          imageSize,
                                                          backgroundColor,
                                                          tagStyleByTagType);
            if (!foundErrorResult.IsSuccess)
                return Result.Fail<TagCloudContext>(foundErrorResult.Error);

            var imageCenter = new Point(imageSize.Value.Width / 2, imageSize.Value.Height / 2);
            var tagCloud = cloudOptions.ConstructCloud(backgroundColor.Value, tagStyleByTagType.Value);
            var cloudContext = new TagCloudContext(imageFilename.Value,
                                                   imageSize.Value,
                                                   cloudVocabulary.Value.ToArray(),
                                                   tagCloud,
                                                   new CircularCloudLayouter(imageCenter),
                                                   excludedWords.Value.ToHashSet()).AsResult();
            return cloudContext;
        }

        private static Result<string> VerifyFilename(string filename)
        {
            var invalidCharacterIndex = filename.IndexOfAny(Path.GetInvalidFileNameChars());

            return filename.FailIf(() => invalidCharacterIndex > -1,
                                   () => $@"Filename contains invalid character '{filename[invalidCharacterIndex]
                                       }' by index {invalidCharacterIndex}");
        }

        private Result<IEnumerable<string>> ParseCloudVocabulary(string cloudVocabularyFilename) =>
            cloudVocabularyFilename is null
                ? Enumerable.Empty<string>().AsResult()
                : vocabularyParser.GetCloudVocabulary(cloudVocabularyFilename);

        private static Result<Size> ParseImageSize(string imageSize)
        {
            var match = imageSizeValidator.Match(imageSize);

            if (!(match.Success && match.Length == imageSize.Length))
                return Result.Fail<Size>($"Invalid image size format '{imageSize}'. Correct format example: '800x600'");

            var width = int.Parse(match.Groups[WidthGroupName].Value);
            var height = int.Parse(match.Groups[HeightGroupName].Value);

            return new Size(width, height).AsResult();
        }

        private static Result<Color> ParseColor(string hexColor)
        {
            if (!IsFullMatch(hexColorPattern, hexColor))
                return Result.Fail<Color>(
                    $"Invalid background color format '{hexColor}'. Correct format example: '#FFA05A2C'");

            var argbColor = int.Parse(hexColor.Replace("#", ""), NumberStyles.HexNumber);

            return Color.FromArgb(argbColor).AsResult();
        }

        private static Result<Dictionary<TagType, TagStyle>> TagStylesParse(
            int groupsCount, string mutualFont, string fontSizes, string tagColors)
        {
            const string separator = "_";

            var sizes = ParseSizes(fontSizes, separator).ToArray();
            var colors = ParseColors(tagColors, separator).ToArray();

            if (sizes.Length != groupsCount || colors.Length != groupsCount)
                return Result.Fail<Dictionary<TagType, TagStyle>>(
                    $@"Wrong format: sizes count has to be equal to colors count and GroupsCount={groupsCount}{
                            Environment.NewLine}Sizes correct format example: '60_22'{
                            Environment.NewLine}Colors correct format example: '#FFFFFFFF_#FFFF6600'");

            var tagStyleByTagType = Enumerable.Range(0, groupsCount)
                .ToDictionary(tagTypeAsNumber => (TagType)tagTypeAsNumber, GetStyle);

            if (tagStyleByTagType.Values.First().Font.Name != mutualFont)
                return Result.Fail<Dictionary<TagType, TagStyle>>(
                    $"Specified mutual font name '{mutualFont}' is not installed on the current machine.");

            return tagStyleByTagType.AsResult();

            TagStyle GetStyle(int tagTypeAsNumber)
            {
                var font = new Font(mutualFont, sizes[tagTypeAsNumber]);
                return new TagStyle(colors[tagTypeAsNumber], font);
            }
        }

        private static IEnumerable<int> ParseSizes(string fontSizes, string sizesSeparator)
        {
            foreach (var sizeString in fontSizes.Split(sizesSeparator.ToCharArray()))
                if (int.TryParse(sizeString, out var size) && size > 0)
                    yield return size;
        }

        private static IEnumerable<Color> ParseColors(string tagColors, string sizesSeparator) =>
            tagColors.Split(sizesSeparator.ToCharArray())
                .Select(ParseColor)
                .Where(resultColor => resultColor.IsSuccess)
                .Select(resultColor => resultColor.Value);

        private static bool IsFullMatch(Regex pattern, string input)
        {
            var match = pattern.Match(input);
            return match.Success && match.Length == input.Length;
        }
    }
}