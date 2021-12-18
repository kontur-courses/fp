using Autofac;
using ResultMonad;
using TagsCloudDrawer.ImageCreator;
using TagsCloudVisualization.Drawable.Displayer;

namespace TagsCloudVisualization.Module
{
    public static class TagsCloudDrawerModuleExtensions
    {
        public static Result<ContainerBuilder> RegisterTagsClouds(this ContainerBuilder builder,
            TagsCloudVisualisationSettings settings)
        {
            return TagsCloudDrawerModule.Create(settings)
                .Then(module =>
                {
                    builder.RegisterModule(module);
                    return builder;
                });
        }

        public static Result<ContainerBuilder> RegisterImageCreation(this Result<ContainerBuilder> result,
            string filename)
        {
            return result
                .Then(builder =>
                {
                    builder.Register(c => new ImageCreationDisplayer(filename, c.Resolve<IImageCreator>()))
                        .As<IDrawableDisplayer>();
                    return builder;
                });
        }
    }
}