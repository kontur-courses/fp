using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using TagCloud.Forms;
using TagCloud.Settings;
using TagCloud.TagCloudVisualization.Visualization;
using TagCloud.Words;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TagCloud
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Program))).AsImplementedInterfaces()
                    .SingleInstance();
                builder.RegisterType<ImageBox>().AsSelf().SingleInstance();
                builder.RegisterType<WordManager>().AsSelf();
                builder.RegisterType<ApplicationWindow>().AsSelf();
                builder.RegisterType<TagCloudVisualizer>().AsSelf();
                builder.RegisterInstance(new SettingsLoader());
                builder.RegisterInstance(new NHunspellSettings
                {
                    AffFile = "Dictionaries/ru_RU.aff",
                    DictFile = "Dictionaries/ru_RU.dic"
                }).SingleInstance();
                builder.RegisterInstance(new ImageSettings());
                builder.RegisterInstance(new FontSettings());
                var mainForm = builder.Build().ResolveOptional<ApplicationWindow>();
                mainForm.RunApplication();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

}