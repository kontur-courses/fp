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
    public static Result<None> CreateSettingsFile()
    {
        try
        {
            var settings = new LayoutSettings
            (
                PathToExcludedWords: null,
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
            return ResultExtension.Ok();
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<None>($"{e.GetType().Name} {e.Message}");
        }
    }

    public static Result<None> SaveSettingsFile(LayoutSettings settings)
    {
        try
        {
            var jsonSettings = Serialize(settings);
            using var fileWriter = new StreamWriter("./TagsCloudSettings.json");
            fileWriter.Write(jsonSettings);
            return ResultExtension.Ok();
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<None>($"{e.GetType().Name} {e.Message}");
        }
    }

    public static Result<None> TryGetLayoutSettings(out LayoutSettings settings)
    {
        try
        {
            using var fileReader = new StreamReader("./TagsCloudSettings.json");
            var jsonSettings = fileReader.ReadToEnd();
            settings = Deserialize<LayoutSettings>(jsonSettings);
            return ResultExtension.Ok();
        }
        catch (Exception e)
        {
            settings = default;
            return ResultExtension.Fail<None>($"{e.GetType().Name} {e.Message}");
        }
    }
}