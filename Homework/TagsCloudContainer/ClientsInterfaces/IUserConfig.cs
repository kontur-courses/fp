using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.PaintConfigs;
using TagsCloudContainer.TextParsers;

namespace TagsCloudContainer.ClientsInterfaces
{
    public interface IUserConfig
    {
        string InputFilePath { get; set; }
        string InputFileFormat { get; set; }
        string OutputFilePath { get; set; }
        string TagsFontName { get; set; }
        int TagsFontSize { get; set; }
        string[] Tags { get; set; }


        ImageFormat ImageFormat { get; set; }
        Size ImageSize { get; set; }
        Point ImageCenter { get; set; }
        ISpiral Spiral { get; set; }
        IColorScheme ColorScheme { get; set; }
        ISourceReader SourceReader { get; set; }
        IHandlerConveyor HandlerConveyor { get; set; }
        ITextParser TextParser { get; set; }
    }
}
