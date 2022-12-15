using TagCloudContainer.Filters;
using TagCloudContainer.Formatters;
using TagCloudContainer.FrequencyWords;
using TagCloudContainer.Parsers;
using TagCloudContainer.Readers;
using TagCloudContainer.TagsWithFont;
using TagCloudGraphicalUserInterface.Settings;

namespace TagCloudGraphicalUserInterface.Interfaces
{
    public interface IPresetsSettings
    {
        public bool txtReader { get; set; }
        public bool Filtered { get; set; }
        public bool ToLowerCase { get; set; }
        public bool PaletteUse { get; set; }
        public IFileReader Reader { get; }
        public IFileParser Parser { get; }
        public IFilter Filter { get; }
        public IWordFormatter Formatter { get; }
        public IFrequencyCounter FrequencyCounter { get; }
        public IFontSizer FontSizer { get; }
        public ICloudDrawer Drawer { get; }

    }
}
