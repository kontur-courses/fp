using System;
using System.Drawing;

namespace TagsCloudContainerCore.LayoutSettingsDir;

[Serializable]
public record LayoutSettings(string FontName,
    float FontMaxSize,
    float FontMinSize,
    int Step,
    Size PictureSize,
    string BackgroundColor,
    string FontColor,
    string PicturesFormat,
    string PathToExcludedWords,
    float MinAngle
)
{
    public override string ToString()
    {
        return $"Font name: {FontName}\n" +
               $"Max font size: {FontMaxSize}\n" +
               $"Min font size: {FontMinSize}\n" +
               $"Font Color: {FontColor}\n" +
               $"Background Color: {BackgroundColor}\n" +
               $"Picture format: {PicturesFormat}\n" +
               $"Picture Size: {PictureSize}\n\n" +
               "Algorithm settings:\n" +
               $"\tAlgorithm step length: {Step}\n" +
               $"\tAlgorithm min angle: {MinAngle}\n\n" +
               $"Path to exclude words file: {PathToExcludedWords}\n";
    }
}