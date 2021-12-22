using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using CommandDotNet;
using TagsCloudContainerCore.Helpers;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.LayoutSettingsDir;
using TagsCloudContainerCore.Result;
using TagsCloudContainerCore.TagCloudMaker;
using WinCloudLayouterConsoleUI.DI;
using WinCloudLayouterConsoleUI.WindowsDependencies;

namespace WinCloudLayouterConsoleUI;

// ReSharper disable once InconsistentNaming
// ReSharper disable once ClassNeverInstantiated.Global
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822", MessageId = "Пометьте члены как статические")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
internal class WinConsoleUI
{
    public static void Main(string[] args)
    {
        if (!File.Exists("./TagsCloudSettings.xml"))
        {
            JsonSettingsHelper.CreateSettingsFile();
        }

        new AppRunner<WinConsoleUI>().Run(args);
    }

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [Command("font",
        Description = "Настройка шрифта для отрисовки облака",
        Usage = "%AppName% font --name <string> --minsize <float> --maxsize <float> --color <HEX>")]
    public void SetFont(
        [Named("name", Description = "Имя используемого шрифта")]
        string? name = null,
        [Named("maxsize", Description = "Максимальный размер шрифта")]
        float? maxSize = null,
        [Named("minsize", Description = "Минимальный размер шрифта")]
        float? minSize = null,
        [Named("color", Description = "Цвет шрифта")]
        string? color = null
    )
    {
        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => DICloudLayouterContainerFactory.GetContainer(settings))
            .TryResolve(out IFontChecker fontChecker)
            .Then(_ => settings)
            .SetFontColor(color)
            .SetFontName(name, fontChecker)
            .SetFontSize(maxSize)
            .SetFontSize(minSize, true)
            .SaveSettingsFile()
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle);
    }


    [Command("settings", Description = "Выводит текущие настройки")]
    public void PrintSettings()
    {
        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle, "\n" + settings);
    }


    [Command("reset", Description = "Сбрасывает все настройки до начальных")]
    public void ResetSettings()
    {
        JsonSettingsHelper.CreateSettingsFile()
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle, "[НАСТРОЙКИ СБРОШЕНЫ]");
    }

    [Command("circle", Description = "Настройки алгоритма")]
    public void SetAlgorithmSettings(
        [Named("angle", Description = "Минимальный угол в градусах с которым будем шагать по окружности. " +
                                      "Должен быть >0")]
        float? minAngle = null,
        [Named("step", Description = "Шаг, на который будем увеличивать радиус. Должен быть > 0")]
        int? step = null)
    {
        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => settings)
            .SetStep(step)
            .SetMinAngle(minAngle)
            .SaveSettingsFile()
            .OnSuccess(SuccessHandle)
            .OnFail(ExceptionHandle);
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
            ExceptionHandle("Длина и ширина должны быть положительными числами");
            return;
        }

        var size = new Size(width, height);
        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => { settings = settings with { PictureSize = size }; })
            .Then(_ => JsonSettingsHelper.SaveSettingsFile(settings))
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle, $"Установлен размер {size}");
    }


    [Command("background", Description = "Устанавливает цвет заднего фона")]
    public void SetBackgroundColor(
        [Named("color")] string color)
    {
        var colorRegex = new Regex("^[0-1a-fA-F]{6}$");
        if (!colorRegex.IsMatch(color))
        {
            ExceptionHandle("Вы ввели не правильную кодировку цвета.\n" +
                            " Используйте преставление HEX, например \"FF01AB\"");
        }

        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => { settings = settings with { BackgroundColor = color }; })
            .Then(_ => JsonSettingsHelper.SaveSettingsFile(settings))
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle, $"Установлен цвет фона: \"{color}\"\n");
    }


    [Command("exclude", Description = "Добавляем файл с исключёнными словами. " +
                                      "Файл должен содержать слова записанные через пробел")]
    public void ExcludeWordsFromFile(
        [Positional(Description = "Путь до файла")]
        string path)
    {
        if (!File.Exists(path))
        {
            ExceptionHandle($"Файл {path} не существует");
            return;
        }

        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => { settings = settings with { PathToExcludedWords = path }; })
            .Then(_ => JsonSettingsHelper.SaveSettingsFile(settings))
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle);
    }

    [Command("format", Description = "Устанавливает формат выходного изображения")]
    public void SetFormat([Positional(Description = "Формат. Поддерживаемые: jpeg, png, bmp")] string format)
    {
        if (!WinSaver.SupportedFormats.ContainsKey(format))
        {
            ExceptionHandle($"Неподдерживаемый формат: {format}\nПоддерживаемые: jpeg, png, bmp");
            return;
        }

        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => { settings = settings with { PicturesFormat = format }; })
            .Then(_ => JsonSettingsHelper.SaveSettingsFile(settings))
            .OnFail(ExceptionHandle)
            .OnSuccess(SuccessHandle);
    }

    [DefaultCommand]
    public void DrawCloud(
        [Named("in", Description = "Путь к файлу со списком тегов")]
        string pathIn,
        [Named("out", Description = "Путь к файлу, куда сохранить изображение")]
        string pathOut)
    {
        JsonSettingsHelper.TryGetLayoutSettings(out var settings)
            .Then(_ => DICloudLayouterContainerFactory.GetContainer(settings))
            .TryResolve(out ITagCloudMaker cloudMaker)
            .TryResolve(out IPainter painter)
            .TryResolve(out IBitmapHandler bitmapHandler)
            .Then(_ => FileReaderHelper.ReadLinesFromFile(pathIn))
            .Then(words => cloudMaker.GetTagsToRender(words))
            .Then(tags => painter.Paint(tags))
            .Then(pic => bitmapHandler.Handle(pic, pathOut, settings.PicturesFormat))
            .OnSuccess(SuccessHandle, $"\nСохранено в:\n{pathOut}")
            .OnFail(ExceptionHandle);
    }

    private static void ExceptionHandle(string information)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n[ОШИБКА]\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(information);
        Console.ResetColor();
    }

    private static void SuccessHandle(string information = "")
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[ГОТОВО]\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(information);
        Console.ResetColor();
    }
}