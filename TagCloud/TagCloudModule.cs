using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Autofac;
using TagCloud.Infrastructure.Graphics;
using TagCloud.Infrastructure.Layout;
using TagCloud.Infrastructure.Layout.Environment;
using TagCloud.Infrastructure.Layout.Strategies;
using TagCloud.Infrastructure.Settings;
using TagCloud.Infrastructure.Settings.UISettingsManagers;
using TagCloud.Infrastructure.Text;
using TagCloud.Infrastructure.Text.Conveyors;
using TagCloud.Infrastructure.Text.Information;
using Module = Autofac.Module;

namespace TagCloud
{
    public class TagCloudModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .Except<ImageSaver>()
                .Except<IConveyor<string>>()
                .Except<Random>(registrationBuilder => registrationBuilder.AsSelf().SingleInstance())
                .Except<Settings>(rb => 
                    rb.AsSelf().AsImplementedInterfaces().SingleInstance())
                .Except<LowerCaseConveyor>()
                .Except<WordTypeConveyor>()
                .Except<WordCounterConveyor>()
                .Except<WordThresholdConveyor>()
                .Except<InterestingWordsConveyor>()
                .Except<WordFontSizeConveyor>()
                .Except<WordSizeConveyor>()
                .Except<OrderConveyor>();

            builder.RegisterType<LowerCaseConveyor>().As<IConveyor<string>>();
            var myStemPath = GetReleasePath("mystem");
            builder.RegisterType<WordTypeConveyor>()
                .As<IConveyor<string>>()
                .WithParameter(new TypedParameter(typeof(string), myStemPath));
            builder.RegisterType<WordCounterConveyor>().As<IConveyor<string>>();
            builder.RegisterType<WordThresholdConveyor>().As<IConveyor<string>>();
            builder.RegisterType<InterestingWordsConveyor>().As<IConveyor<string>>();
            builder.RegisterType<WordFontSizeConveyor>().As<IConveyor<string>>();
            builder.RegisterType<WordSizeConveyor>().As<IConveyor<string>>();
            builder.RegisterType<OrderConveyor>().As<IConveyor<string>>();

            builder.RegisterType<ImageSaver>();
            builder.RegisterType<ColorPicker>();
            builder.RegisterType<ColorConverter>();
        }

        public static Settings GetDefaultSettings()
        {
            var size = new Size(500, 500);
            return new Settings
            {
                ExcludedTypes = new[] {WordType.CONJ, WordType.SPRO, WordType.PR},
                Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "examples", "input.txt"),
                WordCountThreshold = 2,
                Increment = 1,
                Width = size.Width,
                Height = size.Height,
                MinFontSize = 5,
                MaxFontSize = 50,
                Center = new Point(size.Width / 2, size.Height / 2),
                ImagePath = Path.Combine(".", "drawing.bmp"),
                FontFamily = new FontFamily("Arial"),
                Format = ImageFormat.Bmp,
                ColorMap = new Dictionary<WordType, Color>()
            };
        }

        public static string GetReleasePath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Release", filename);
        }
    }
}