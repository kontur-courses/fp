using System.Text;
using Spire.Doc;
using TagCloud.ResultImplementation;

namespace TagCloud.FileConverter.Implementation;

public class ConvertToTxt : IFileConverter
{
    public Result<string> Convert(string path)
    {
        var document = new Document();
        try
        {
            document.LoadFromFile(path);
            document.SaveToTxt("temp.txt", Encoding.UTF8);
            return "temp.txt";
        }
        catch (Exception e)
        {
            return Result.Fail<string>($"{e.Message}");
        }
    }
}