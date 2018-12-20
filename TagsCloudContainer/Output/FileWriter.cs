using System.Drawing;
using System.IO;

namespace TagsCloudContainer.Output
{
    public class FileWriter : IWriter
    {
        public void WriteToFile(byte[] bytes, string name)
        {
            var image = Image.FromStream(new MemoryStream(bytes));
            image.Save(name);
        }
    }
}