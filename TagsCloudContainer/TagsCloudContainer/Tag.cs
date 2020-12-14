using System;
using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.TagsCloudContainer
{
    public class Tag
    {
        public Tag(string text, Rectangle rectangle, Font font, Brush textColor)
        {
            Text = text;

            Result.Ok(rectangle)
                .Then(ValidateRectangleCoordinateX)
                .Then(ValidateRectangleCoordinateY)
                .Then(ValidateRectangleHeight)
                .Then(ValidateRectangleWidth)
                .OnFail(e => throw new ArgumentException(e));

            Result.Ok(font)
                .Then(ValidateFontIsNotNull)
                .OnFail(e => throw new ArgumentException(e));

            Result.Ok(textColor)
                .Then(ValidateBrushIsNotNull)
                .OnFail(e => throw new ArgumentException(e));

            Rectangle = rectangle;
            Font = font;
            TextColor = textColor;
        }

        public string Text { get; }
        public Rectangle Rectangle { get; }
        public Font Font { get; }
        public Brush TextColor { get; }

        public Tag ChangeFontFamily(string fontFamily)
        {
            return new Tag(Text, Rectangle, new Font(fontFamily, Font.Size), TextColor);
        }

        public Tag ChangeTextColor(Brush color)
        {
            return new Tag(Text, Rectangle, Font, color);
        }

        private Result<Rectangle> ValidateRectangleWidth(Rectangle rectangle)
        {
            return Validate(rectangle, x => x.Width < 0,
                "Rectangle width should not be negative, but was: " + rectangle.Width);
        }

        private Result<Rectangle> ValidateRectangleHeight(Rectangle rectangle)
        {
            return Validate(rectangle, x => x.Height < 0,
                "Rectangle height should not be negative, but was: " + rectangle.Height);
        }

        private Result<Rectangle> ValidateRectangleCoordinateX(Rectangle rectangle)
        {
            return Validate(rectangle, x => x.X < 0,
                "Rectangle coordinate x should not be negative, but was: " + rectangle.X);
        }

        private Result<Rectangle> ValidateRectangleCoordinateY(Rectangle rectangle)
        {
            return Validate(rectangle, x => x.Y < 0,
                "Rectangle coordinate y should not be negative, but was: " + rectangle.Y);
        }

        private Result<Font> ValidateFontIsNotNull(Font font)
        {
            return Validate(font, x => x == null, "Font was null reference");
        }

        private Result<Brush> ValidateBrushIsNotNull(Brush brush)
        {
            return Validate(brush, x => x == null, "Brush was null reference");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}