using TagCloudContainer.Core.Interfaces;

namespace TagCloudContainer.Configs;

public class TagCloudContainerConfig : ITagCloudContainerConfig
{
    public bool NeedValidateWords { get; set; } = true;
    public Size StandartFontSize { get; set; }
    public string WordsFilePath { get; set; }
    public string ExcludeWordsFilePath { get; set; }
    public string ImageName { get; set; }
}