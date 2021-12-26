using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autofac;
using TagsCloudContainer.Layouters;
using TagsCloudContainer.Painting;
using TagsCloudContainer.Preprocessors;
using TagsCloudContainer.UI;
using TagsCloudVisualization;
using Module = Autofac.Module;

namespace TagsCloudContainer
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Надо ли разбивать на разные модули?
            LoadVisualizatorDependencies(builder);
            LoadContainerDependencies(builder);
        }

        private void LoadContainerDependencies(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Except<IUiAction>()
                .AsImplementedInterfaces()
                .SingleInstance()
                .PublicOnly();

            builder.RegisterType<TagCircularLayouter>().As<TagLayouter>();
            builder.RegisterType<WordsCountParser>().AsSelf();
            builder.RegisterType<TagsPreprocessor>().AsSelf();
            builder.RegisterType<TagPainter>().AsSelf();
            builder.RegisterType<Processor>().AsSelf();

            DefaultInactivePreprocessorsRegistrator(builder);
        }

        private void LoadVisualizatorDependencies(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.FullName.Contains(nameof(TagsCloudVisualization))))
                .AsImplementedInterfaces()
                .SingleInstance()
                .PublicOnly();

            // Сделал так потому что протаскивать новый класс CloudSettings в Cloud как-то глупо,
            // делать провайдер тоже не очень
            // Так что наверное это будет лучше
            var cloud = new Cloud(new PointF());
            var spiral = new Spiral(new PointF());
            builder.RegisterInstance(cloud).AsImplementedInterfaces();
            builder.RegisterInstance(spiral).AsImplementedInterfaces();

            builder.RegisterType<Tag>();
        }

        private static void DefaultInactivePreprocessorsRegistrator(ContainerBuilder builder)
        {
            var hashSet = new HashSet<string>();
            CustomTagsFilter.RelevantTag selector = t => true;
            builder.RegisterInstance(hashSet).AsSelf();
            builder.RegisterInstance(selector)
                .AsSelf();
        }
    }
}