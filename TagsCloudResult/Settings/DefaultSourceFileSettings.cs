using System.IO;

namespace TagsCloudResult.Settings
{
    public class DefaultSourceFileSettings : ISourceFileSettings
    {
        public string FilePath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "1984_lines.txt");
    }
}