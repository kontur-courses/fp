using System.Collections.Generic;
using System.Linq;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Enums;

namespace TagsCloudContainer.Filters
{
    public class MyStemFilter : IFilter
    {
        private readonly GramPartsEnum[] allowedWorldType;
        private readonly IMysteam mysteam;

        public MyStemFilter(GramPartsEnum[] allowedWorldType, IMysteam mysteam)
        {
            this.allowedWorldType = allowedWorldType;
            this.mysteam = mysteam;
        }

        public IEnumerable<string> Filtering(IEnumerable<string> tokens)
        {
            var result = mysteam.GetWords(string.Join(" ", tokens))
                .Where(el => el.Lexems.Count > 0 && allowedWorldType.Contains(el.Lexems[0].GramPart))
                .Select(t => t.SourceWord.Text);
            return result;
        }
    }
}