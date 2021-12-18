using System.Drawing;
using ResultMonad;

namespace TagsCloudVisualization
{
    public readonly struct PositiveSize
    {
        public readonly int Width;
        public readonly int Height;

        private PositiveSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static implicit operator Size(PositiveSize size) => new(size.Width, size.Height);

        public static Result<PositiveSize> Create(int width, int height)
        {
            return Result.Ok()
                .Validate(() => width > 0, "Expected width to be positive")
                .Validate(() => height > 0, "Expected height to be positive")
                .ToValue(new PositiveSize(width, height));
        }
    }
}