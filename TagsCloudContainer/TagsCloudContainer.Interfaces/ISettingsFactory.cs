using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface ISettingsFactory
{
    Result<Settings> Build();
}