using System.Collections.Generic;

namespace TagsCloud.ProgramOptions
{
    public interface IFilterOptions
    {
        public string MystemLocation { get; set; }

        public IEnumerable<string> BoringWords { get; set; }
    }
}