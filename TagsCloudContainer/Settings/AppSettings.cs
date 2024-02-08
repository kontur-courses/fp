using System.Reflection;

namespace TagsCloudContainer.Settings;

public class AppSettings: IAppSettings
{
    public string ProjectDirectory { get; set; }

    public AppSettings()
    {
        var location = Assembly.GetExecutingAssembly().Location;
        var path = Path.GetDirectoryName(location);
        ProjectDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), "../../../"));
    }
    
    public string InputFile { get; set; }

    public string OutputFile { get; set; }
}