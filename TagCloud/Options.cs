using CommandLine;

namespace TagCloud
{
    public class Options
    {
        [Option('i', "interface", Required = true, HelpText = "Set user interface type.")]
        public UiType UserInterfaceType { get; set; }
    }

}