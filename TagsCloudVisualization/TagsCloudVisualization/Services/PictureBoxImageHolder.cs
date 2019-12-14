using System.Drawing;
using System.Windows.Forms;
using ErrorHandler;

namespace TagsCloudVisualization.Services
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder 
    {
        public Result<None> SetImage(Bitmap image)
        {
            Image = image;
            return Result.Ok();
        }

        public Result<Image> GetImage()
        {
            return Image ?? Result.Fail<Image>("ITag cloud was not created");
        }
    }
}