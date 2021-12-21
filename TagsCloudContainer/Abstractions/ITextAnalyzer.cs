using ResultOf;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Abstractions;

public interface ITextAnalyzer : IService
{
    Result<ITextStats> AnalyzeText();
}