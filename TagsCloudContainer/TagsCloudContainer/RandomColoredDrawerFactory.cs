using System.Drawing;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class RandomColoredDrawerFactory : IDrawerFactory
{
    public Result<IDrawerProvider> Build(DrawerSettings drawerSettings)
    {
        return drawerSettings.SuccessIfCast<DrawerSettings, RandomColoredDrawerSettings>()
            .Bind(settings => Result.Success(new DrawerProvider(settings)))
            .Bind(provider => Result.Success((IDrawerProvider)provider));
    }

    private class DrawerProvider : IDrawerProvider
    {
        private readonly RandomColoredDrawerSettings settings;

        public DrawerProvider(RandomColoredDrawerSettings settings)
        {
            this.settings = settings;
        }

        public Result<IDrawer> Provide(ILayouterAlgorithmProvider layouterAlgorithmProvider, Graphics graphics)
        {
            return new RandomColoredDrawer(settings, graphics, layouterAlgorithmProvider);
        }
    }
}