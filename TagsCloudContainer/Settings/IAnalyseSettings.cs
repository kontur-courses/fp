namespace TagsCloudContainer.Settings;

public interface IAnalyseSettings: ISettings
{
    public string[] ValidSpeechParts { get; set; }
}