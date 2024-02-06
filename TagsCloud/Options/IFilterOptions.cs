namespace TagsCloud.Options;

public interface IFilterOptions
{
    bool OnlyRussian { get; }
    HashSet<string> LanguageParts { get; }
    HashSet<string> ExcludedWords { get; }
}