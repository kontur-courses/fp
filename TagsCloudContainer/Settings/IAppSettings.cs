namespace TagsCloudContainer.Settings;

public interface IAppSettings
{
    public string InputFile { get; set; }

    public string OutputFile { get; set; }

    public string ProjectDirectory { get; set; }
}