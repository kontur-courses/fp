using System.IO;

namespace TagsCloudResult.Settings
{
    public class DefaultSourceFileSettings : ISourceFileSettings
    {
        public string FilePath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "1984_lines.txt");
    }
}