using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using CommandDotNet;
using TagsCloudContainerCore.Helpers;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.LayoutSettingsDir;
using TagsCloudContainerCore.Result;
using TagsCloudContainerCore.TagCloudMaker;
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
    private readonly IFontChecker fontChecker;
    private readonly ITagCloudMaker cloudMaker;
    private readonly IBitmapHandler bitmapHandler;
    private readonly IPainter painter;
    private LayoutSettings settings;

    public WinConsoleUI(IFontChecker fontChecker,
        ITagCloudMaker cloudMaker,
        IBitmapHandler bitmapHandler,
        IPainter painter,
        LayoutSettings settings)
    {
        this.fontChecker = fontChecker;
        this.cloudMaker = cloudMaker;
        this.bitmapHandler = bitmapHandler;
        this.painter = painter;
        this.settings = settings;
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
        ResultExtension.Ok(settings)
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
        SuccessHandle(settings.ToString());
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
        ResultExtension.Ok(settings)
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

        settings = settings with { PictureSize = size };
        JsonSettingsHelper.SaveSettingsFile(settings)
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

        {
            settings = settings with { BackgroundColor = color };
            JsonSettingsHelper.SaveSettingsFile(settings)
                .OnFail(ExceptionHandle)
                .OnSuccess(SuccessHandle, $"Установлен цвет фона: \"{color}\"\n");
        }
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

        settings = settings with { PathToExcludedWords = path };
        JsonSettingsHelper.SaveSettingsFile(settings)
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

        settings = settings with { PicturesFormat = format };
        JsonSettingsHelper.SaveSettingsFile(settings)
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
        var cloud = FileReaderHelper.ReadLinesFromFile(pathIn)
            .Then(words => cloudMaker.GetTagsToRender(words))
            .OnFail(ExceptionHandle);

        if (!cloud.IsSuccess) return;

        var limRectangles = cloud.GetValueOrThrow().Select(x => x.LimitingRectangle);
        var cloudSize = GeometryHelper.GetCommonSize(limRectangles);

        if (cloudSize.Height > settings.PictureSize.Height || cloudSize.Width > settings.PictureSize.Width)
        {
            WarningHandle("Размер облака больше размера картинки. \n" +
                          $"Размер облака: {cloudSize}");
        }

        cloud.Then(tags => painter.Paint(tags))
            .Then(pic => bitmapHandler.Handle(pic, pathOut, settings.PicturesFormat))
            .OnSuccess(SuccessHandle, $"\nСохранено в:\n{pathOut}")
            .OnFail(ExceptionHandle)
            .GetValueOrThrow()
            .Dispose();
    }

    internal static void ExceptionHandle(string information)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n[FAIL]\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(information);
        Console.ResetColor();
    }

    internal static void WarningHandle(string information)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n[WARNING]\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(information);
        Console.ResetColor();
    }

    internal static void SuccessHandle(string information = "")
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[SUCCESS]\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(information);
        Console.ResetColor();
    }
}