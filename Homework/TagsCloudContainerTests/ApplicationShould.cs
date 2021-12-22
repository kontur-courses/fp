using System;
using System.IO;
using Autofac;
using CLI;
using ContainerConfigurers;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace CloudContainerTests
{
    [TestFixture]
    internal class ApplicationShould
    {
        [Test]
        public void Create_Output_File()
        {
            var config = new Client(new[] {"--words", "hi"}).UserConfig;
            var container = new AutofacConfigurer(config).GetContainer();

            using (var scope = container.BeginLifetimeScope())
            {
                var painter = scope.Resolve<CloudPainter>();
                painter.Draw();
            }

            File.Exists("tagcloud" + ".Png").Should().BeTrue();
            File.Delete("tagcloud" + ".Png");
        }

        [Test]
        public void Throw_When_Any_Word_Is_Outside_Image()
        {
            var config = new Client(new[]
            {
                "--words", "happy", "birthday", "mister", "president",
                "-s", "30", "-w", "200", "-h", "200"
            }).UserConfig;
            var container = new AutofacConfigurer(config).GetContainer();

            using (var scope = container.BeginLifetimeScope())
            {
                var painter = scope.Resolve<CloudPainter>();
                FluentActions.Invoking(() => painter.Draw().GetValueOrThrow())
                    .Should().Throw<ArgumentException>();
            }
        }
    }
}
