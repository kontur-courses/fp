using Aspose.Drawing.Imaging;

namespace TagCloud.Domain.Settings;

public class FileSettings
{
    private string _outFileName = "cloud";
    private string _outPathToFile = "../../../TagCloudImages";
    private string _fileFromWithPath = "../../../src/source.txt";

    public string OutFileName
    {
        get => _outFileName;
        set
        {
            if (Path.GetInvalidFileNameChars().Any(value.Contains))
                throw new ArgumentException($"Name {value} is invalid");

            _outFileName = value;
        }
    }

    public string OutPathToFile
    {
        get => _outPathToFile;
        set
        {
            if (Path.GetInvalidPathChars().Any(value.Contains))
                throw new ArgumentException($"Path {value} is invalid");

            _outPathToFile = value;
        }
    }

    public string FileFromWithPath
    {
        get => _fileFromWithPath;
        set
        {
            if (!File.Exists(value))
                throw new ArgumentException($"There is no file by path {value}");

            _fileFromWithPath = value;
        }
    }
    
    public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;
}