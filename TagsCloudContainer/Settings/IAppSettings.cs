namespace TagsCloudContainer.Settings;

public interface IAppSettings: ISettings
{
    public string InputFile { get; set; }

    public string OutputFile { get; set; }
    
    public string ProjectDirectory { get; set; }
}