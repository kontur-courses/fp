using CommandLine;

namespace TagsCloudContainer
{
    public class Options
    {
        [Option('r', "read", MetaValue = "FILE", Required = true, HelpText = "Full wordlist file name.")]
        public string File { get; set; }
        [Option('h', "height", Default = 1024, HelpText = "Image height.")]
        public int Height { get; set; }
        [Option('w', "width", Default = 1024, HelpText = "Image width.")]
        public int Width { get; set; }
        [Option('f', "font", Default = "Arial", HelpText = "Font name.")]
        public string Font { get; set; }
        [Option('s', "size", Default = 20, HelpText = "Font size.")]
        public int Size { get; set; }
        [Option('c', "color", Default = "Red", HelpText = "Font color. (Does not work)")]
        public string Color { get; set; }
        [Option('i', "image", Default = "image.png", HelpText = "Image name.")]
        public string Image { get; set; }
    }
}
