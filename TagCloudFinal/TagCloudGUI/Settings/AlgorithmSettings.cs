using System.ComponentModel;
using System.Runtime.Serialization;
using TagCloudContainer.FrequencyWords;
using TagCloudContainer.Interfaces;
using TagCloudContainer.Readers;
using TagCloudContainer.TagsWithFont;
using TagCloudGUI.Interfaces;

namespace TagCloudGUI.Settings
{
    public class AlgorithmSettings : IAlgorithmSettings, IPresetsSettings
    {
        public AlgorithmSettings(IFontSettings fontSettings, ICloudDrawer drawer, IFileReader reader, IFileParser parser,
            IWordFormatter formatter, IFrequencyCounter frequency, IFontSizer sizer)
        {
            FontSettings = fontSettings;

            Reader = reader;
            Parser = parser;
            Formatter = formatter;
            FrequencyCounter = frequency;
            FontSizer = sizer;
            Drawer = drawer;
        }

        [Browsable(false)] public IFontSettings FontSettings { get; set; }

        [DisplayName("Шрифт")]
        public string FontFamilyName
        {
            get => FontSettings.Font.Name;
            set
            {
                try
                {
                    FontSettings.Font = new FontFamily(value);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        [DisplayName("Максимальный шрифт")]
        public int MaxFontSize
        {
            get => FontSettings.MaxFontSize;
            set
            {
                if (!IsFontSizesCorrect(FontSettings.MinFontSize, value))
                    MessageBox.Show("Incorrect font size");
                else
                    FontSettings.MaxFontSize = value;
            }
        }

        [DisplayName("Минимальный шрифт")]
        public int MinFontSize
        {
            get => FontSettings.MinFontSize;
            set
            {
                if (!IsFontSizesCorrect(value, FontSettings.MaxFontSize))
                    MessageBox.Show("Incorrect font size");
                else
                    FontSettings.MinFontSize = value;
            }
        }

        [DisplayName("Путь к файлу")] public string FilePath { get; set; }

        [DisplayName("Удалять мусор")] public Switcher Filtered { get; set; }
        [DisplayName("Нижний регистр")] public Switcher ToLowerCase { get; set; }

        [Browsable(false)] public IFileReader Reader { get; }
        [Browsable(false)] public IFileParser Parser { get; }
        [Browsable(false)] public IWordFormatter Formatter { get; }
        [Browsable(false)] public IFrequencyCounter FrequencyCounter { get; }
        [Browsable(false)] public IFontSizer FontSizer { get; }
        [Browsable(false)] public ICloudDrawer Drawer { get; }

        public bool IsFontSizesCorrect(int min, int max)
            => max > 0 && min > 0 && max > min;
    }
}
