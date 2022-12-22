namespace TagCloud.FileReader;

public interface IFileReader
{
    string[] Read(string path);
}