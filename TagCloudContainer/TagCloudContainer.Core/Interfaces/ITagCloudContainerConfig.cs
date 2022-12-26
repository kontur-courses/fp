namespace TagCloudContainer.Core.Interfaces;

public interface ITagCloudContainerConfig
{
    public bool NeedValidateWords { get; set; }
    public Size StandartFontSize { get; set; }
    public string WordsFilePath { get; set; }
    public string ExcludeWordsFilePath { get; set; }
    public string ImageName { get; set; }
}