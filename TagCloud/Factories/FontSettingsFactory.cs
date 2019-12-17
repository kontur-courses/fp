using System;
using TagCloud.Models;

namespace TagCloud.IServices
{
    public class FontSettingsFactory : IFontSettingsFactory
    {
        public FontSettings CreateFontSettingsOrThrow(string fontName)
        {
            return  fontName is null 
                ? throw new ArgumentException("Параметр FontName не определен") 
                : new FontSettings(fontName);
        }
    }
}