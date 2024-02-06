using TagsCloud.Entities;

namespace TagsCloud.Options;

public interface IInputProcessorOptions : IFilterOptions
{
    bool ToInfinitive { get; }
    CaseType WordsCase { get; }
}