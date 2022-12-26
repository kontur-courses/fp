using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface IDrawerFactory
{
    Result<IDrawerProvider> Build(
        DrawerSettings drawerSettings);
}