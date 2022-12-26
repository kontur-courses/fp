using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Autofac;
using TagsCloudContainer;

namespace TagsCloudGUI
{
    public class DIBuilder
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainForm>().As<Form>();
            builder.RegisterType<DefaultDrawer>().As<IDrawer>();
            builder.RegisterType<InputFileHandler>().As<IInputTextProvider>().SingleInstance();
            builder.RegisterType<CircularCloudSettingsProvider>().AsSelf().SingleInstance();
            builder.RegisterType<DefaultDrawerSettingsProvider>().AsSelf().SingleInstance();
            builder.RegisterType<DefaultRectangleArranger>().As<IRectangleArranger>();
            builder.RegisterType<WordFilter>().As<IWordFilter>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            return builder.Build();
        }
    }
}