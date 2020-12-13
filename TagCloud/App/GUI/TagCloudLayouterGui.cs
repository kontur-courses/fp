﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Gdk;
using Gtk;
using TagCloud.Infrastructure;
using TagCloud.Infrastructure.Graphics;
using TagCloud.Infrastructure.Settings.UISettingsManagers;
using TagCloud.Infrastructure.Text.Information;
using Application = Gtk.Application;
using Button = Gtk.Button;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Label = Gtk.Label;
using Settings = TagCloud.Infrastructure.Settings.Settings;
using Window = Gtk.Window;

namespace TagCloud.App.GUI
{
    internal class TagCloudLayouterGui : IApp
    {
        private readonly Func<Settings> settingsFactory;
        private readonly IEnumerable<IInputManager> settingsManagers;
        private readonly IImageGenerator imageGenerator;
        private readonly ImageSaver imageSaver;
        private readonly ColorConverter colorConverter;

        public TagCloudLayouterGui(Func<Settings> settingsFactory, IEnumerable<IInputManager> settingsManagers, IImageGenerator imageGenerator, ImageSaver imageSaver, ColorConverter colorConverter)
        {
            this.settingsFactory = settingsFactory;
            this.settingsManagers = settingsManagers;
            this.imageGenerator = imageGenerator;
            this.imageSaver = imageSaver;
            this.colorConverter = colorConverter;
        }

        public void Run()
        {
            settingsFactory().Import(Program.GetDefaultSettings());

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
            window.SetDefaultSize(900, 900);
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
            var scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(box);
            window.Add(scrolledWindow);
            window.ShowAll();
        }

        private Widget GetWidget(IInputManager manager)
        {
            const int padding = 10;
            var settings = new VBox();
            var inputBox = new VBox();
            var infoBox = new HBox();
            var title = new Label
            {
                UseMarkup = true, 
                Markup = $"<span font='large' weight='bold'>{manager.Title}</span>"
            };

            infoBox.PackStart(title, false, false, 0);
            infoBox.PackStart(new VSeparator(), true, false, 0);
            infoBox.PackStart(new Label(manager.Help), false, false, 0);
            settings.PackStart(infoBox, false, false, padding);
            settings.PackStart(inputBox, false, false, 0);
            switch (manager)
            {
                case IOptionsManager optionsManager:
                    AddManger(optionsManager, inputBox, padding);
                    break;
                case IMultiOptionsManager multiOptionsManager:
                    AddManger(multiOptionsManager, inputBox, padding);
                    break;
                default:
                    AddManger(manager, inputBox, padding);
                    break;
            }
            return settings;
        }

        private void AddManger(IOptionsManager manager, Box containerBox, uint padding)
        {
            var dropdown = new ComboBoxText();
            containerBox.PackStart(dropdown, true, true, padding);
            foreach (var (option, id) in manager
                .GetOptions()
                .Select((s, i) => (s, i)))
            {
                dropdown.Append(option, option);
            }

            dropdown.SetActiveId(manager.GetSelectedOption());
            
            dropdown.Changed += (o, args) => OnValueSet(
                manager,
                () => dropdown.ActiveText,
                value => dropdown.SetActiveId(manager.GetSelectedOption()))(o, args);
        }
        
        private void AddManger(IInputManager manager, Box containerBox, uint padding)
        {
            var table = new TextTagTable();
            var buffer = new TextBuffer(table);
            var textBox = new TextView(buffer);
            containerBox.PackStart(textBox, true, true, padding);
            textBox.Shown += (o, args) => { buffer.Text = manager.Get(); };
            textBox.FocusOutEvent += (o, args) => OnValueSet(
                manager, 
                () => buffer.Text, 
                value => buffer.Text = value)(o, args);
        }

        private void AddManger(IMultiOptionsManager manager, Box containerBox, uint padding)
        {
            var options = new List<Widget>();
            var managerOptions = manager.GetOptions();
            foreach (var optionName in managerOptions.Keys)
            {
                var hBox = new HBox(); 
                var dropdown = BuildDropdown(manager, managerOptions[optionName]);
                var wordType = (WordType) Enum.Parse(typeof(WordType), optionName);
                if (settingsFactory().ColorMap.TryGetValue(wordType, out var chosenColor))
                {
                    dropdown.SetActiveId(GetColorName(chosenColor));
                }
                dropdown.Changed += (o, args) => OnValueSet(
                    manager,
                    () => $"{optionName} = {dropdown.ActiveText}",
                    value =>
                    {
                        if (manager.GetSelectedOptions().TryGetValue(optionName, out var selectedValue))
                            dropdown.SetActiveId(selectedValue);
                    })(o, args);
                
                hBox.PackStart(dropdown, true, true, 0);
                hBox.PackStart(new Label(optionName), true, true, 0);
                containerBox.PackStart(hBox, true, true, 1);
            }
        }

        private string GetColorName(Color color) => colorConverter.ConvertToString(color.Name);

        private ComboBoxText BuildDropdown(IMultiOptionsManager manager, IEnumerable<string> options)
        {
            var dropdown = new ComboBoxText();
            foreach (var option in options)
            {
                dropdown.Append(option, option);
            }

            return dropdown;
        }


        private EventHandler OnValueSet(IInputManager manager, Func<string> getInput, Action<string> setValue)
        {
            return (o, args) =>  {
                var result = manager.TrySet(getInput());
                if (!result.IsSuccess) 
                    ShowInfoWindow(ErrorLevel.Warning, result.Error);

                setValue(manager.Get());
            };
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
            }).ReplaceError(_ => $"Displaying of format {format} is not supported");
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