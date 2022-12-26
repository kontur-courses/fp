using System.Drawing;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class ClassicDrawerFactory : IDrawerFactory
{
    public Result<IDrawerProvider> Build(
        DrawerSettings drawerSettings)
    {
        return drawerSettings is not ClassicDrawerSettings classicDrawerSettings
            ? Result.Failure<IDrawerProvider>($"{nameof(drawerSettings)} is not match {nameof(ClassicDrawerSettings)}")
            : new ClassicDrawerProvider(classicDrawerSettings);
    }

    private class ClassicDrawerProvider : IDrawerProvider
    {
        private readonly ClassicDrawerSettings settings;

        public ClassicDrawerProvider(ClassicDrawerSettings settings)
        {
            this.settings = settings;
        }

        public Result<IDrawer> Provide(ILayouterAlgorithmProvider layouterAlgorithmProvider, Graphics graphics)
        {
            return new ClassicDrawer(settings, graphics, layouterAlgorithmProvider);
        }
    }
}