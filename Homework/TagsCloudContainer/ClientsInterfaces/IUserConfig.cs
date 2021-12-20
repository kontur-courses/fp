using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.PaintConfigs;
using TagsCloudContainer.TextParsers;

namespace TagsCloudContainer.ClientsInterfaces
{
    public interface IUserConfig
    {
        string[] Tags { get; set; }
        string OutputFilePath { get; set; }
        string TagsFontName { get; set; }
        int TagsFontSize { get; set; }
        ImageFormat ImageFormat { get; set; }
        Size ImageSize { get; set; }
        Point ImageCenter { get; set; }
        IColorScheme TagsColors { get;  set; }
        ISourceReader SourceReader { get; set; }
        IHandlerConveyor HandlerConveyor { get; set; }
        ISpiral Spiral { get; set; }
        ITextParser TextParser { get; set; }
    }
}
