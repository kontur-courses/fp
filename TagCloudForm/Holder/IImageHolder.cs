using System.Drawing;
using ErrorHandling;
using TagCloud.Visualization;

namespace TagCloudForm.Holder
{
    public interface IImageHolder
    {
        void UpdateUi();
        Result<None> RecreateImage(ImageSettings settings);
        void SaveImage(string fileName);

        Image Image { set; }
    }
}