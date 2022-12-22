using System.Text;
using Spire.Doc;
using TagsCloud.FileConverter;

namespace TagCloud.FileConverter.Implementation;

public class ConvertToTxt : IFileConverter
{
    public string Convert(string path)
    {
        var document = new Document();
        document.LoadFromFile(path);
        document.SaveToTxt("temp.txt", Encoding.UTF8);
        return "temp.txt";
    }
}