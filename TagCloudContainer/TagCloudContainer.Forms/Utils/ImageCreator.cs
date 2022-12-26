using System.Drawing.Imaging;
using TagCloudContainer.Core.Interfaces;

namespace TagCloudContainer;

public class ImageCreator : IImageCreator
{
    private readonly ISelectedValues _selectedValues;
    
    public ImageCreator(ISelectedValues selectedValues)
    {
        _selectedValues = selectedValues;
    }
    
    public void Save(Form form, string path)
    {
        using (Bitmap bitmap = new Bitmap(_selectedValues.ImageSize.Width, _selectedValues.ImageSize.Height))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(form.DesktopLocation.X, form.DesktopLocation.Y), new Point(0, 0), _selectedValues.ImageSize);
            }
            bitmap.Save(path, ImageFormat.Png);
        }
    }
}