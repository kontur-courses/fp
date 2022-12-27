using System.Drawing;
using TagCloudContainer.Interfaces;

namespace TagCloudContainer.Models
{
    public class FontSettings : IFontSettings
    {
        private int maxFontSize = 72;
        private int minFontSize = 32;

        public int MaxFontSize 
        { 
            get => maxFontSize; 
            set => maxFontSize = value;
            
        }

        public int MinFontSize
        {
            get => minFontSize;
            set => minFontSize = value;
        }

        public FontFamily Font { get; set; } = new FontFamily("Arial");
    }
}
