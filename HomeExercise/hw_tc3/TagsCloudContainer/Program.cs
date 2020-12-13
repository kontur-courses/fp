using System;
using System.IO;
using Autofac;

namespace TagsCloudContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.GetDirectoryName(
                Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            var creator = Configurator.GetContainer()
                .Then(cont => cont.BeginLifetimeScope())
                .Then(scope => scope.Resolve<TagsCloudCreator>())
                .OnFail(err => throw new Exception(err))
                .Value;
            Result.Of(() => creator.SetFontRandomColor())
                .Then(res => creator.SetImageFormat("png"))
                .Then(res => creator.SetFontFamily("Comic Sans MS"))
                .Then(res => creator.SetImageSize(500))
                .Then(res => creator.AddStopWord("aba"))
                .Then(res => creator.Create(Path.Combine(path, "input.txt"), path, "Cloud2"))
                .OnFail(err => throw new Exception(err));
        }
    }
}
