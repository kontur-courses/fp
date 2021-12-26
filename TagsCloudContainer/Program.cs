using System;
using System.IO;
using System.Linq;
using Autofac;
using TagsCloudContainer.Common.Result;
using TagsCloudContainer.UI;
using TagsCloudContainer.UI.Menu;

namespace TagsCloudContainer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            var preContainer = RegisterDependensiesInPreContainer(builder).Build();
            builder.RegisterModule<AutofacModule>();
            SetUpUi(preContainer);
        }
        
        private static ContainerBuilder RegisterDependensiesInPreContainer(ContainerBuilder builder)
        {
            var preBuilder = new ContainerBuilder();
            preBuilder.RegisterInstance(Console.Out).As<TextWriter>();
            preBuilder.RegisterInstance(Console.In).As<TextReader>();
            preBuilder.RegisterType<ConsoleResultHandler>().AsImplementedInterfaces();
            preBuilder.RegisterInstance(builder).As<ContainerBuilder>();
            RegisterActions(preBuilder);
            return preBuilder;
        }

        private static void SetUpUi(IContainer preContainer)
        {
            using (var scope = preContainer.BeginLifetimeScope())
            {
                var menu = scope.Resolve<IMenuCreator>().Menu;
                while (true)
                {
                    menu.ChooseCategory();
                }
            }
        }

        private static void RegisterActions(ContainerBuilder builder)
        {
            var action = typeof(UiAction);
            var actions = AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.FullName.Contains("TagsCloudContainer"))
                .GetTypes()
                .Where(t => action.IsAssignableFrom(t))
                .ToArray();
            builder.RegisterTypes(actions).AsImplementedInterfaces();
            builder.RegisterType<MenuCreator>().AsImplementedInterfaces();
        }
    }
}
