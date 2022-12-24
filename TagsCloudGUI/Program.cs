using System;
using System.Windows.Forms;
using Autofac;
using TagsCloudContainer;

namespace TagsCloudGUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var builder = new ContainerBuilder();
            builder.RegisterType<MainForm>().As<Form>();
            builder.RegisterType<DefaultDrawer>().As<IDrawer>();
            builder.RegisterType<InputFileHandler>().As<IInputTextProvider>().SingleInstance();
            builder.RegisterType<CircularCloudSettingsProvider>().AsSelf().SingleInstance();
            builder.RegisterType<DefaultDrawerSettingsProvider>().AsSelf().SingleInstance();
            builder.RegisterType<DefaultRectangleArranger>().As<IRectangleArranger>();
            builder.RegisterType<WordFilter>().As<IWordFilter>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            var container = builder.Build();
            
            Application.Run(container.Resolve<Form>());
        }
    }
}