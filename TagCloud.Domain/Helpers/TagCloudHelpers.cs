public static class TagCloudHelpers
{
    public static Result<double> GetMultiplier(int value, int min, int max)
    {
        return Result.Of(() => ((double)value - min) / (max - min));
    }
}