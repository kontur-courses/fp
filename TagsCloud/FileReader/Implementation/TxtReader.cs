namespace TagCloud.FileReader.Implementation;

public class TxtReader : IFileReader
{
    public string[] Read(string path) => File.ReadAllLines(path);
}