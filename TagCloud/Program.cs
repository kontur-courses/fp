using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using TagCloud.Forms;
using TagCloud.Settings;
using TagCloud.TagCloudVisualization.Visualization;
using TagCloud.Words;

namespace TagCloud
{
    internal class Program
    {   
        [STAThread]
        private static void Main()
        {
            try
            {
                var settings = Properties.Settings.Default;
                var fontSettings = GetFontSettings(settings);
                var imageSettings = GetImageSettings(settings);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Program))).AsImplementedInterfaces()
                    .SingleInstance();
                builder.RegisterType<ImageBox>().AsSelf().SingleInstance();
                builder.RegisterType<WordManager>().AsSelf();

                builder.RegisterType<ApplicationWindow>().AsSelf();
                builder.RegisterType<TagCloudVisualizer>().AsSelf();                
                builder.RegisterInstance(new NHunspellSettings
                {
                    AffFile = "Dictionaries/ru_RU.aff", 
                    DictFile = "Dictionaries/ru_RU.dic"
                }).SingleInstance();
                builder.RegisterInstance(imageSettings).AsSelf().SingleInstance();
                builder.RegisterInstance(fontSettings).AsSelf().SingleInstance();
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
        
        private static FontSettings GetFontSettings(Properties.Settings settings)
        {
            return Result.Of(() => new FontSettings
            {
                FontFamily = new FontFamily(settings.FontFamily),
                MinFontSize = settings.MinFontSize,
                MaxFontSize = settings.MaxFontSize
            })
                .RefineError($"Failed to load file 'Settings.settings'. Path {Environment.CurrentDirectory}")
                .GetValueOrThrow();
        }

        private static ImageSettings GetImageSettings(Properties.Settings settings)
        {
            return Result.Of(() => new ImageSettings
            {
                Width = settings.Width,
                Height = settings.Height,
                TextColor = settings.TextColor,
                BackgroundColor = settings.BackgroudColor
            })                
                .RefineError($"Failed to load file 'Settings.settings'. Path {Environment.CurrentDirectory}")
                .GetValueOrThrow();
        }
    }
}