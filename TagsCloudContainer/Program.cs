namespace TagsCloudContainer;
public class Program
{
    public static void Main(string[] args)
    {
        args = new[] { "--files", "bigbook.txt",
            "--height", "10000",
            "--width", "10000",
            "--center", "5000, 5000",
            "--word-limit", "3000",
            "--color", "red",
            "--implement-with", $"IWordFilter none" };
        InitializationHelper.RunWithArgs(args);
    }
}
