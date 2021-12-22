using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.LayoutSettingsDir;

// ReSharper disable once UnusedType.Global
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class LayoutSettingsExtension
{
    public static Result<LayoutSettings> SetFontColor(this Result<LayoutSettings> settings, string? fontColor)
    {
        if (fontColor is null || !settings.IsSuccess)
        {
            return settings;
        }

        var colorRegex = new Regex("^[0-9a-fA-F]{6}$");

        if (!colorRegex.IsMatch(fontColor))
        {
            return ResultExtension.Fail<LayoutSettings>("Вы ввели не правильную кодировку цвета.\n" +
                                                        " Используйте представление HEX, например \"FF01AB\"");
        }

        return ResultExtension.Ok(settings.GetValueOrThrow() with { FontColor = fontColor });
    }

    public static Result<LayoutSettings> SetFontSize(this Result<LayoutSettings> settings,
        float? size,
        bool isSetMinSize = false)
    {
        if (size is null || !settings.IsSuccess)
        {
            return settings;
        }

        if (size is <= 0 or >= 200)
        {
            return ResultExtension.Fail<LayoutSettings>("Размер шрифта должен быть больше 0 но не больше 200");
        }

        return isSetMinSize
            ? settings.GetValueOrThrow() with { FontMinSize = size.Value }
            : settings.GetValueOrThrow() with { FontMaxSize = size.Value };
    }

    public static Result<LayoutSettings> SetFontName(this Result<LayoutSettings> settings,
        string? name,
        IFontChecker fontChecker)
    {
        if (name is null || !settings.IsSuccess)
        {
            return settings;
        }

        return !fontChecker.IsFontInstalled(name)
            ? ResultExtension.Fail<LayoutSettings>($"Шрифт \"{name}\" не установлен в системе")
            : ResultExtension.Ok(settings.GetValueOrThrow() with { FontName = name });
    }

    public static Result<LayoutSettings> SetMinAngle(this Result<LayoutSettings> settings, float? angle)
    {
        if (angle is null || !settings.IsSuccess)
        {
            return settings;
        }

        return angle <= 0
            ? ResultExtension.Fail<LayoutSettings>("Угол должен быть больше 0")
            : ResultExtension.Ok(settings.GetValueOrThrow() with { MinAngle = angle.Value });
    }

    public static Result<LayoutSettings> SetStep(this Result<LayoutSettings> settings, int? step)
    {
        if (step is null || !settings.IsSuccess)
        {
            return settings;
        }

        return step <= 0
            ? ResultExtension.Fail<LayoutSettings>("Шаг должен быть положительным числом")
            : ResultExtension.Ok(settings.GetValueOrThrow() with { Step = step.Value });
    }
}