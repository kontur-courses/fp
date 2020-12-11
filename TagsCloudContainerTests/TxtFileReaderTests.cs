using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.DataReader;

namespace TagsCloudContainerTests
{
    internal class TxtFileReaderTests
    {
        private readonly string txtFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.txt");

        [Test]
        public void TxtReader_ShouldReadLines()
        {
            new TxtFileReader(txtFilePath)
                .ReadLines()
                .GetValueOrThrow()
                .ToArray()
                .Should()
                .BeEquivalentTo("Это", "Txt", "Файл");
        }
    }
}
