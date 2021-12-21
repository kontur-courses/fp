using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using Autofac;
using CommandDotNet;
using TagsCloudContainerCore.Helpers;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.TagCloudMaker;
using WinCloudLayouterConsoleUI.WindowsDependencies;

namespace WinCloudLayouterConsoleUI;

// ReSharper disable once InconsistentNaming
// ReSharper disable once ClassNeverInstantiated.Global
[SuppressMessage("Interoperability", "CA1416", MessageId = "Проверка совместимости платформы")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822", MessageId = "Пометьте члены как статические")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class WinConsoleUI
{
    public static void Main(string[] args)
    {
        if (!File.Exists("./TagsCloudSettings.xml"))
        {
            JsonSettingsHelper.CreateSettingsFile();
        }

        new AppRunner<WinConsoleUI>().Run(args);
    }

    [Command("font",
        Description = "Настройка шрифта для отрисовки облака",
        Usage = "%AppName% font --name <string>\"font name\" --size <int> --color <HEX>")]
    public void SetFont(
        [Named("name", Description = "Имя используемого шрифта")]
        string? name = null,
        [Named("maxsize", Description = "Максимальный размер шрифта")]
        int? maxSize = null,
        // ReSharper disable once StringLiteralTypo
        [Named("minsize", Description = "Минимальный размер шрифта")]
        int? minSize = null,
        [Named("color", Description = "Цвет шрифта")]
        string? color = null
    )
    {
        var settings = JsonSettingsHelper.GetLayoutSettings();
        var fontName = settings.FontName;
        var fontColor = settings.FontColor;
        var fontMaxSize = settings.FontMaxSize;
        var fontMinSize = settings.FontMinSize;
        if (!string.IsNullOrEmpty(name))
        {
            using var checkFont = new Font(name, 10);

            if (!checkFont.Name.Equals(name))
            {
                throw new ArgumentException($"Шрифт {name} не установлен в системе\n{checkFont.Name}");
            }

            fontName = name;
            Console.WriteLine($"Установлен шрифт \"{name}\"\n");
        }

        if (!string.IsNullOrEmpty(color))
        {
            var colorRegex = new Regex("^[0-1a-fA-F]{6}$");
            if (!colorRegex.IsMatch(color))
            {
                throw new ArgumentException("Вы ввели не правильную кодировку цвета.\n" +
                                            " Используйте представление HEX, например \"FF01AB\"");
            }

            fontColor = color.ToUpperInvariant();
            Console.WriteLine($"Установлен цвет шрифта: \"{color}\"\n");
        }

        if (maxSize != null)
        {
            if (maxSize is <= 0 or > 200)
            {
                throw new ArgumentException("Размер шрифта должен быть больше 0 и не превышать 200");
            }

            fontMaxSize = maxSize.Value;
            Console.WriteLine($"Установлен максимальный размер шрифта: \"{settings.FontMaxSize}\"\n");
        }

        if (minSize != null)
        {
            if (minSize is <= 0 or > 200)
            {
                throw new ArgumentException("Размер шрифта должен быть больше 0 и не превышать 200");
            }

            fontMinSize = minSize.Value;
            Console.WriteLine($"Установлен максимальный размер шрифта: \"{settings.FontMaxSize}\"\n");
        }

        settings = settings with
        {
            FontMaxSize = fontMaxSize,
            FontMinSize = fontMinSize,
            FontColor = fontColor,
            FontName = fontName
        };

        JsonSettingsHelper.SaveSettingsFile(settings);
        PrintSettings();
    }


    [Command("settings", Description = "Выводит текущие настройки")]
    public void PrintSettings()
    {
        var settings = JsonSettingsHelper.GetLayoutSettings();
        Console.WriteLine("\n" + settings);
    }


    [Command("reset", Description = "Сбрасывает все настройки до начальных")]
    public void ResetSettings()
    {
        JsonSettingsHelper.CreateSettingsFile();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n[Настройки сброшены]".ToUpper());
        Console.ResetColor();
        PrintSettings();
        Console.ResetColor();
    }

    [Command("circle", Description = "Настройки алгоритма")]
    public void SetAlgorithmSettings(
        [Named("angle", Description = "Минимальный угол в градусах с которым будем шагать по окружности. " +
                                      "Должен быть >0")]
        float? minAngle = null,
        [Named("step", Description = "Шаг, на который будем увеличивать радиус. Должен быть > 0")]
        int? step = null)
    {
        var settings = JsonSettingsHelper.GetLayoutSettings();

        if (minAngle != null)
        {
            if (minAngle < 0)
            {
                throw new ArgumentException("Угол должен быть положительным");
            }

            settings = settings with { MinAngle = minAngle.Value };
            Console.WriteLine($"Минимальный угол установлен на {minAngle.Value} градусов");
        }

        if (step != null)
        {
            if (step < 0)
            {
                throw new ArgumentException("Размер шага должен быть положительным");
            }

            settings = settings with { Step = step.Value };
            Console.WriteLine($"Шаг радиуса установлен на {step.Value} пикселей");
        }

        JsonSettingsHelper.SaveSettingsFile(settings);

        PrintSettings();
    }

    // ReSharper disable once StringLiteralTypo
    [Command("picsize", Description = "Устанавливает размер выходного изображения")]
    public void SetPictureSize(
        [Named("width")] int width,
        [Named("height")] int height
    )
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("Длина и ширина должны быть положительными числами");
        }

        var size = new Size(width, height);
        var settings = JsonSettingsHelper.GetLayoutSettings();

        settings = settings with { PictureSize = size };

        Console.WriteLine($"Установлен размер изображения {size}");

        JsonSettingsHelper.SaveSettingsFile(settings);

        PrintSettings();
    }

    [Command("background", Description = "Устанавливает цвет заднего фона")]
    public void SetBackgroundColor(
        [Named("color")] string color)
    {
        var colorRegex = new Regex("^[0-1a-fA-F]{6}$");
        if (!colorRegex.IsMatch(color))
        {
            throw new ArgumentException("Вы ввели не правильную кодировку цвета.\n" +
                                        " Используйте преставление HEX, например \"FF01AB\"");
        }

        var settings = JsonSettingsHelper.GetLayoutSettings();
        settings = settings with { BackgroundColor = color };
        JsonSettingsHelper.SaveSettingsFile(settings);
        Console.WriteLine($"Установлен цвет фона: \"{color}\"\n");
    }

    [Command("exclude", Description = "Добавляем файл с исключёнными словами. " +
                                      "Файл должен содержать слова записанные через пробел")]
    public void ExcludeWordsFromFile(
        [Positional(Description = "Путь до файла")]
        string path)
    {
        if (!File.Exists(path))
        {
            throw new ArgumentException($"Файл {path} не существует");
        }

        var settings = JsonSettingsHelper.GetLayoutSettings();
        settings = settings with { PathToExcludedWords = path };

        JsonSettingsHelper.SaveSettingsFile(settings);
        PrintSettings();
    }

    [Command("format", Description = "Устанавливает формат выходного изображения")]
    public void SetFormat([Positional(Description = "Формат. Поддерживаемые: jpeg, png, bmp")] string format)
    {
        if (!WinSaver.SupportedFormats.ContainsKey(format))
        {
            throw new FormatException($"Неподдерживаемый формат: {format}\nПоддерживаемые: jpeg, png, bmp");
        }

        var settings = JsonSettingsHelper.GetLayoutSettings();
        settings = settings with { PicturesFormat = format };

        JsonSettingsHelper.SaveSettingsFile(settings);
    }

    [DefaultCommand]
    public void DrawCloud(
        [Named("in", Description = "Путь к файлу со списком тегов")]
        string pathIn,
        [Named("out", Description = "Путь к файлу, куда сохранить изображение")]
        string pathOut)
    {
        var settings = JsonSettingsHelper.GetLayoutSettings();

        var container = DICloudLayouterContainerFactory.GetContainer(settings);

        var layouter = container.Resolve<ITagCloudMaker>();
        var painter = container.Resolve<IPainter>();

        var rawTags = FileReaderHelper.ReadLinesFromFile(pathIn);
        var tagsToRender = layouter.GetTagsToRender(rawTags);

        using var picture = painter.Paint(tagsToRender);

        var bmpHandler = container.Resolve<IBitmapHandler>();
        bmpHandler.Handle(picture, pathOut, settings.PicturesFormat);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[ГОТОВО]\n");
        Console.ResetColor();
    }
}