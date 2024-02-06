using System.Drawing;
using TagsCloud.Distributors;
using TagsCloud.Entities;
using TagsCloud.WordFontCalculators;

namespace TagsCloud.Layouters;

public class CircularCloudLayouter : ILayouter
{
    public Point Center { get; private set; }
    private readonly List<Tag> _tags;
    private readonly IDistributor _distributor;
    private int _leftBorder;
    private int _rightBorder;
    private int _topBorder;
    private int _bottomBorder;

    public CircularCloudLayouter(IDistributor distributor)
    {
        this._distributor = distributor;
        this.Center = new Point();
        _tags = new();
    }

    public Result<Cloud> CreateTagsCloud(Dictionary<string, Font> tagsDictionary)
    {
        if (!tagsDictionary.Any())
            return Result.Fail<Cloud>("Dictionary cannot be empty\n");

        foreach (var tag in tagsDictionary)
        {
            var rectangle = new Rectangle();

            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1)))
            {
                var sizeF = g.MeasureString(tag.Key, tag.Value);
                rectangle.Size = new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
            }

            rectangle.Location = _distributor.GetNextPosition().GetValueOrThrow();
            while (CheckIntersection(rectangle))
            {
                rectangle.Location = _distributor.GetNextPosition().GetValueOrThrow();
            }

            rectangle = CompressRectangleToCenter(rectangle);
            UpdateImageSize(rectangle);
            var newtag = new Tag(rectangle, tag.Value, tag.Key);
            _tags.Add(newtag);
        }

        var cloud = new Cloud(_tags, GetImageSize());
        return Result.Ok(cloud);
    }

    private Size GetImageSize()
    {
        var size = new Size(Math.Abs(_rightBorder - _leftBorder), Math.Abs(_topBorder - _bottomBorder));
        return size;
    }

    private void UpdateImageSize(Rectangle rec)
    {
        var right = rec.X + rec.Width / 2;
        var left = rec.X - rec.Width / 2;
        var top = rec.Y + rec.Height / 2;
        var bottom = rec.Y - rec.Height / 2;

        _rightBorder = right > _rightBorder ? right : _rightBorder;
        _leftBorder = left < _leftBorder ? left : _leftBorder;
        _topBorder = top > _topBorder ? top : _topBorder;
        _bottomBorder = bottom < _bottomBorder ? bottom : _bottomBorder;
    }

    private bool CheckIntersection(Rectangle currentRectangle)
    {
        return _tags.Any(rec => currentRectangle.IntersectsWith(rec.TagRectangle));
    }

    private Rectangle CompressRectangleToCenter(Rectangle rectangle)
    {
        var changes = 1;
        while (changes > 0)
        {
            rectangle = CompressByAxis(rectangle, true, out changes);
            rectangle = CompressByAxis(rectangle, false, out changes);
        }

        return rectangle;
    }

    private Rectangle CompressByAxis(Rectangle rectangle, bool isByX, out int changes)
    {
        changes = 0;
        var stepX = rectangle.X < Center.X ? 1 : -1;
        var stepY = rectangle.Y < Center.Y ? 1 : -1;

        while ((isByX && Math.Abs(rectangle.X - Center.X) > 0) ||
               (!isByX && Math.Abs(rectangle.Y - Center.Y) > 0))
        {
            var newRectangle = isByX
                ? new Rectangle(new Point(rectangle.X + stepX, rectangle.Y), rectangle.Size)
                : new Rectangle(new Point(rectangle.X, rectangle.Y + stepY), rectangle.Size);

            if (!CheckIntersection(newRectangle))
            {
                rectangle = newRectangle;
                changes++;
                continue;
            }

            break;
        }

        return rectangle;
    }
}