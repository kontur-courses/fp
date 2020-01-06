using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class WordsToTagsParser : IWordsToTagsParser
    {
        private readonly IFontSettingsFactory fontSettingsFactory;

        public WordsToTagsParser(IFontSettingsFactory fontSettingsFactory)
        {
            this.fontSettingsFactory = fontSettingsFactory;
        }

        public Result<List<Tag>> GetTags(Dictionary<string, int> words, ImageSettings imageSettings)
        {
            return Result.Of(() => fontSettingsFactory.CreateFontSettingsOrThrow(imageSettings.FontName))
                .Then(fs => words.Select(s => new Tag(s.Key, s.Value, GetFont(fs, s.Value,words)))
                    .OrderByDescending(t => t.Count)
                    .ToList());
        }

        private Font GetFont(FontSettings fontSettings, int count, Dictionary<string,int> words)
        {
            var fontSize= fontSettings.defaultFontSize + count * 300 / words.Count ;
            return new Font(fontSettings.fontFamilyName, (float) fontSize, fontSettings.fontStyle);
        }
    }
}