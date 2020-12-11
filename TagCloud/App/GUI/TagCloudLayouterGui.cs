using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Gdk;
using Gtk;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Graphics;
using TagCloud.Infrastructure.Settings.UISettingsManagers;
using Application = Gtk.Application;
using Button = Gtk.Button;
using Image = System.Drawing.Image;
using Label = Gtk.Label;
using Settings = TagCloud.Infrastructure.Settings.Settings;
using Window = Gtk.Window;

namespace TagCloud.App.GUI
{
    internal class TagCloudLayouterGui : IApp
    {
        private readonly Func<Settings> settingsFactory;
        private readonly IEnumerable<ISettingsManager> settingsManagers;
        private readonly IImageGenerator imageGenerator;
        private readonly ImageSaver imageSaver;

        public TagCloudLayouterGui(Func<Settings> settingsFactory, IEnumerable<ISettingsManager> settingsManagers, IImageGenerator imageGenerator, ImageSaver imageSaver)
        {
            this.settingsFactory = settingsFactory;
            this.settingsManagers = settingsManagers;
            this.imageGenerator = imageGenerator;
            this.imageSaver = imageSaver;
        }

        public void Run()
        {
            settingsFactory().Import(Program.GetDefaultSettings());
            Console.WriteLine("Default settings imported");

            Application.Init();

            var window = new Window("Tag Cloud Layouter");
            window.DeleteEvent += Close;

            window.Resize(200, 200);
            window.SetPosition(WindowPosition.Center);

            var runButton = new Button("Generate");
            runButton.Clicked += OnGenerateButtonClicked;

            var settingsButton = new Button("Settings");
            settingsButton.Clicked += OnSettingsButtonClicked;

            var box = new HBox();
            window.Add(box);
            var arrow = new Arrow(ArrowType.Right, ShadowType.Out);
            box.PackStart(settingsButton, false, true, 10);
            box.PackStart(arrow, false, true, 10);
            box.PackStart(runButton, false, true, 10);

            window.BorderWidth = 30;
            window.Resizable = true;
            window.SetDefaultSize(100, 100);

            window.ShowAll();
            Application.Run();
        }

        private void OnSettingsButtonClicked(object sender, EventArgs args)
        {
            var window = new Window("Settings");
            window.BorderWidth = 30;
            window.SetPosition(WindowPosition.Mouse);
            window.Resizable = true;
            var box = new VBox();
            foreach (var manager in settingsManagers)
            {
                box.Add(GetWidget(manager));
                box.PackStart(new VSeparator(), false, false, 10);
            }
            
            var okBox = new HBox();
            okBox.PackStart(new Arrow(ArrowType.Right, ShadowType.Out), true, true, 10);
            var okButton = new Button("ok");
            okButton.Pressed += (o, eventArgs) => { window.Close(); };
            okBox.PackStart(okButton, false, false, 0);
            box.PackStart(okBox, true, true, 0);

            window.Add(box);
            window.ShowAll();
        }

        private Widget GetWidget(ISettingsManager manager)
        {
            const int padding = 10;
            var settings = new HBox();
            settings.PackStart(new Label(manager.Help), false, false, padding);
            settings.PackStart(new Label(manager.Title), false, false, padding);
            settings.PackStart(new VSeparator(), false, false, padding);
            var table = new TextTagTable();
            var buffer = new TextBuffer(table);
            var textBox = new TextView(buffer);
            settings.PackStart(textBox, true, true, padding);
            textBox.Shown += (o, args) => { buffer.Text = manager.Get(); };
            textBox.FocusOutEvent += (sender, args) =>
            {
                var result = manager.TrySet(buffer.Text);
                if (!result.IsSuccess)
                {
                    ShowInfoWindow(ErrorLevel.Warning, result.Error);
                }
                buffer.Text = manager.Get();
            };
            return settings;
        }

        private void ShowInfoWindow(ErrorLevel level, string info)
        {
            var window = new Window(level.ToString());
            window.BorderWidth = 30;
            window.SetPosition(WindowPosition.Mouse);
            window.Resizable = true;
            var box = new VBox();
            box.PackStart(new Label(info), false, false, 10);
            box.PackStart(new VSeparator(), false, false, 10);
                    
            var okBox = new HBox();
            var okButton = new Button("ok");
            okButton.Pressed += (o, eventArgs) => { window.Close(); };
            okBox.PackStart(okButton, false, false, 0);
            box.PackStart(okBox, true, true, 0);

            window.Add(box);
            window.ShowAll();
        }


        private static Result<MemoryStream> ToStream(Image image, ImageFormat format)
        {
            return Result.Of(() =>
            {
                var stream = new MemoryStream();
                image.Save(stream, format);
                stream.Position = 0;
                return stream;
            }).RefineError($"Displaying of format {format} is not supported");
        }

        private void OnGenerateButtonClicked(object sender, EventArgs args)
        {
            var window = new Window("Settings");
            window.BorderWidth = 30;
            window.SetPosition(WindowPosition.Mouse);
            window.Resizable = true;
            var box = new VBox();

            var image = imageGenerator.Generate();
            var result = ToStream(image, settingsFactory().Format);
            if (!result.IsSuccess)
            {
                ShowInfoWindow(ErrorLevel.Error, result.Error);
                return;
            }

            var buf = new Pixbuf(result.Value);

            buf = buf.ScaleSimple(500, 500, InterpType.Bilinear);
            var img = new Gtk.Image(buf);
            box.PackStart(img, false, false, 0);

            var okBox = new HBox();
            okBox.PackStart(new Arrow(ArrowType.None, ShadowType.None), true, true, 10);
            var closeButton = new Button("discard");
            closeButton.Pressed += (o, eventArgs) =>
            {
                image.Dispose();
                window.Close();
            };
            var saveButton = new Button("save");
            saveButton.Pressed += (o, eventArgs) =>
            {
                var result = imageSaver.Save(image);
                if (result.IsSuccess)
                {
                    ShowInfoWindow(ErrorLevel.Info, result.Value);
                }
                else
                {
                    ShowInfoWindow(ErrorLevel.Error, result.Error);
                }
                image.Dispose();
                window.Close();
            };
            okBox.PackStart(closeButton, false, false, 0);
            okBox.PackStart(saveButton, false, false, 0);
            box.PackStart(okBox, true, true, 0);

            window.Add(box);
            window.ShowAll();
        }

        private static void Close(object obj, DeleteEventArgs e)
        {
            Console.WriteLine("Closed!");
            Application.Quit();
        }
    }
}