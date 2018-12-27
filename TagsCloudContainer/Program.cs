using System;
using ResultOf;
using TagsCloudContainer.Util;
using TagsCloudContainer.Cloud;

namespace TagsCloudContainer
{
    class Program
    {
        static void Main(string[] args) => Result.Of(() => new AutofacContainer(args))
                .Then(container => new TagCloudRenderer(container.TagCloud, container.FontName, container.Brush)
                .GenerateImage()
                    .Then(img => img.Save(container.OutputPath.GetValueOrThrow())))
                .OnFail(Console.WriteLine);
    }
}
