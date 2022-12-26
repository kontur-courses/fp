using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Extensions
{
    public static class AlgorithmSettingsExtensions
    {
        public static Func<int, Point> GetPointFinderFunction(this AlgorithmSettings source, Point center)
        {
            return (int arg) => new Point(
                (int)(center.X + source.Dr * arg * Math.Cos(source.Fi * arg)),
                (int)(center.Y + source.Dr * arg * Math.Sin(source.Fi * arg)));
        }
    }
}
