using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using TagsCloudContainerCore.LayoutSettingsDir;
using TagsCloudContainerCore.Result;
using static System.Text.Json.JsonSerializer;

namespace TagsCloudContainerCore.Helpers;

// ReSharper disable once UnusedType.Global
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class JsonSettingsHelper
{
    public static Result<LayoutSettings> CreateSettingsFile()
    {
        try
        {
            var settings = new LayoutSettings
            (
                PathToExcludedWords: "",
                // ReSharper disable once StringLiteralTypo
                BackgroundColor: "FFFFFF",
                PicturesFormat: "png",
                FontName: "Arial",
                FontColor: "000000",
                MinAngle: 5,
                Step: 10,
                PictureSize: new Size(1080, 720),
                FontMaxSize: 70,
                FontMinSize: 5
            );

            var jsonSettings = Serialize(settings);
            using var fileWriter = new StreamWriter("./TagsCloudSettings.json");
            fileWriter.Write(jsonSettings);
            return settings;
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<LayoutSettings>($"{e.GetType().Name} {e.Message}");
        }
    }

    public static Result<LayoutSettings> SaveSettingsFile(this Result<LayoutSettings> settings)
    {
        if (!settings.IsSuccess)
        {
            return ResultExtension.Fail<LayoutSettings>(settings.Error);
        }

        try
        {
            var jsonSettings = Serialize(settings.Value);
            using var fileWriter = new StreamWriter("./TagsCloudSettings.json");
            fileWriter.Write(jsonSettings);
            return settings;
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<LayoutSettings>($"{e.GetType().Name} {e.Message}");
        }
    }

    public static Result<LayoutSettings> TryGetLayoutSettings()
    {
        try
        {
            using var fileReader = new StreamReader("./TagsCloudSettings.json");
            var jsonSettings = fileReader.ReadToEnd();

            var settings = Deserialize<LayoutSettings>(jsonSettings)
                           ?? throw new Exception("Ошибка при получении настроек");

            return settings;
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<LayoutSettings>($"{e.GetType().Name} {e.Message}");
        }
    }
}