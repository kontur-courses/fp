using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using static System.Text.Json.JsonSerializer;

namespace TagsCloudContainerCore.Helpers;

// ReSharper disable once UnusedType.Global
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class JsonSettingsHelper
{
    public static void CreateSettingsFile()
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
    }

    public static void SaveSettingsFile(LayoutSettings settings)
    {
        var jsonSettings = Serialize(settings);
        using var fileWriter = new StreamWriter("./TagsCloudSettings.json");
        fileWriter.Write(jsonSettings);
    }

    public static LayoutSettings GetLayoutSettings()
    {
        using var fileReader = new StreamReader("./TagsCloudSettings.json");
        var jsonSettings = fileReader.ReadToEnd();
        return Deserialize<LayoutSettings>(jsonSettings);
    }
}