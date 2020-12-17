using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization.AppSettings
{
    public class WordsSettings
    {
        public string[] ForbiddenWords { get; set; } = new string[0];
        public uint MinLength { get; set; } = 4;
        public uint MaxLength { get; set; } = 10;

        public HashSet<string> GetForbiddenWords => ForbiddenWords.Select(word => word.ToLower()).ToHashSet();
    }
}