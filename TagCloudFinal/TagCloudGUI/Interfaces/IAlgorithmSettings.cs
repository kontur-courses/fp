using TagCloudContainer.Interfaces;

namespace TagCloudGUI.Interfaces
{
    public interface IAlgorithmSettings : IVisualizationCloudSettings, IProviderSettings
    {
        IFontSettings FontSettings { get; set; }
        string FilePath { get; set; }

        FontFamily Font
        {
            get => FontSettings.Font;
            set => FontSettings.Font = value;
        }

        int MaxFontSize
        {
            get => FontSettings.MaxFontSize;
            set => FontSettings.MaxFontSize = value;
        }

        int MinFontSize
        {
            get => FontSettings.MinFontSize;
            set => FontSettings.MinFontSize = value;
        }
    }
}
