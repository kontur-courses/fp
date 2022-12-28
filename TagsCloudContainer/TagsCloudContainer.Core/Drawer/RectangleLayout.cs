using System.Drawing;
using TagsCloudContainer.Core.Options;
using TagsCloudContainer.Core.Results;
using TagsCloudContainer.Core.Layouter;
using TagsCloudContainer.Core.Drawer.Interfaces;
using TagsCloudContainer.Core.Options.Interfaces;
using TagsCloudContainer.Core.Layouter.Interfaces;

namespace TagsCloudContainer.Core.Drawer
{
    public class RectangleLayout : IRectangleLayout
    {
        private readonly ILayouter _layouter;
        private readonly ILayoutDrawer _drawer;
        private readonly IImageOptions _imageOptions;

        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;
        private readonly FontOptions _fontOptions;

        public RectangleLayout(ILayouter layouter, ILayoutDrawer drawer, IImageOptions imageOptions, FontOptions fontOptions)
        {          
            _drawer = drawer;
            _layouter = layouter;
            _fontOptions = fontOptions;
            _imageOptions = imageOptions;

            _bitmap = new Bitmap(imageOptions.Width, imageOptions.Height);
            _graphics = Graphics.FromImage(_bitmap);
        }
        public Result<None> PlaceWords(Dictionary<string, int> words) =>
            Result.Of(() => MakeTags(words))
                .Then(PlaceTags)
                .Then(TagsInsideImage)
                .Then(tags => _drawer.AddTags(tags));

        private IEnumerable<Tag> MakeTags(Dictionary<string, int> words)
        {
            var tags = new List<Tag>();

            foreach (var (word, count) in words)
                tags.Add(new Tag(word, CalculateFontSize(count), _fontOptions.FontFamily, _fontOptions.FontColor));

            return tags;
        }

        private IEnumerable<Tag> PlaceTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                var tagSize = GetTagSize(tag);
                tag.Rectangle = _layouter.PutNextRectangle(tagSize);
            }

            return tags;
        }

        private Size GetTagSize(Tag tag) => _graphics.MeasureString(tag.Text, new Font(tag.FontFamily, tag.FontSize)).ToSize();
       
        private static int CalculateFontSize(int wordCount) => (int)(Math.Log(Math.Max(wordCount, 6), 6) * 10);

        public void DrawLayout()
        {
            _drawer.Draw(_graphics);
        }

        public void SaveLayout()
        {
            var outputDirectory = _imageOptions.ImageOutputDirectory;
            var fullPath = Path.Combine(outputDirectory, _imageOptions.ImageName + _imageOptions.ImageExtension);

            _bitmap.Save(fullPath);
            Console.WriteLine($"Tag cloud visualization saved to file {fullPath}");
        }

        private Result<IEnumerable<Tag>> TagsInsideImage(IEnumerable<Tag> tags) =>
            tags.Any(tag => RectangleInsideImage(tag.Rectangle))
                ? Result.Fail<IEnumerable<Tag>>("Some tags are outside of the image. Change image size.")
                : Result.Ok(tags);

        private bool RectangleInsideImage(Rectangle rectangle) => 
           _imageOptions.Width < rectangle.Right || 0 > rectangle.Left ||
           _imageOptions.Height < rectangle.Bottom || 0 > rectangle.Top;
    }
}
