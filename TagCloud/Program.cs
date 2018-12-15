using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using IniParser;
using IniParser.Model;
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
                var parser = new FileIniDataParser();
                var settings = Result.Of(() => parser.ReadFile("settings.ini"))
                    .RefineError($"Failed to load file 'settings.ini'. Path {Environment.CurrentDirectory}").GetValueOrThrow();
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
        
        private static KeyDataCollection GetSectionSettings(IniData settings, string section)
        {
            return Result.Of(() => settings[section])
                .RefineError($"Section '{section}' was not found in 'settings.ini'. Check {Environment.CurrentDirectory}").GetValueOrThrow();

        }
        private static FontSettings GetFontSettings(IniData settings)
        {
            var sectionSettings = GetSectionSettings(settings, "FontSettings");
            return Result.Of(() => new FontSettings
            {
                FontFamily = Result.Of(() => new FontFamily(sectionSettings["FontFamily"]))
                    .RefineError("Error, trying to get 'FontFamily' value").GetValueOrThrow(),
                MinFontSize = Result.Of(() => int.Parse(sectionSettings["MinFontSize"]))
                    .RefineError("Error, trying to get 'MinFontSize' value").GetValueOrThrow(),
                MaxFontSize = Result.Of(() => int.Parse(sectionSettings["MaxFontSize"]))
                    .RefineError("Error, trying to get 'MaxFontSize' value").GetValueOrThrow()
            }).GetValueOrThrow();
        }

        private static ImageSettings GetImageSettings(IniData settings)
        {
            var sectionSettings = GetSectionSettings(settings, "ImageSettings");
            return Result.Of(() => new ImageSettings
            {
                Width = Result.Of(() => int.Parse(sectionSettings["Width"]))
                    .RefineError("Error, trying to get 'Width' value").GetValueOrThrow(),
                Height = Result.Of(() => int.Parse(sectionSettings["Height"]))
                    .RefineError("Error, trying to get 'Height' value").GetValueOrThrow(),
                TextColor = Result.Of(() => Color.FromName(sectionSettings["TextColor"]))
                    .RefineError("Error, trying to get 'TextColor' value").GetValueOrThrow(),
                BackgroundColor = Result.Of(() => Color.FromName(sectionSettings["BackgroundColor"]))
                    .RefineError("Error, trying to get 'BackgroundColor' value").GetValueOrThrow()
            }).GetValueOrThrow();
        }
    }
}