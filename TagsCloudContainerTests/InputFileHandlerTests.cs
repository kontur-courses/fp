using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer;

namespace TagsCloudContainerTests
{
    public class InputFileHandlerTests
    {
        [Test]
        public void FormFrequencyDictionary_ShouldСorrectFormFrequencyDictionary()
        {
            var result = InputFileHandler.FormFrequencyDictionary(new[] {"mary", "bloody", "Mary", "JUNE"});
            result.Value.Should().BeEquivalentTo(new Dictionary<string, int> {{"mary", 2}, {"bloody", 1}, {"june", 1}});
        }

        [Test]
        public void FormFrequencyDictionary_ShouldReturnErrorOnEmptyInput()
        {
            var result = InputFileHandler.FormFrequencyDictionary(Array.Empty<string>());
            result.Error.Should().Be("Empty file");
        }
    }
}