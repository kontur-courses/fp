using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using TagsCloud.CustomAttributes;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Painters;

[Injection(ServiceLifetime.Singleton)]
public class OneVsRestPainter : IPainter
{
    public ColoringStrategy Strategy => ColoringStrategy.OneVsRest;

    public Result<HashSet<WordTagGroup>> Colorize(HashSet<WordTagGroup> wordGroups, Color[] colors)
    {
        if (colors.Length != 2)
            return ResultExtensions.Fail<HashSet<WordTagGroup>>(
                $"Must be exactly 2 colors for {nameof(OneVsRestPainter)}!");

        var maxFrequency = wordGroups.Max(group => group.Count);

        foreach (var group in wordGroups)
        {
            var textColor = group.Count == maxFrequency ? colors[0] : colors[1];
            group.VisualInfo.TextColor = textColor;
        }

        return wordGroups;
    }
}