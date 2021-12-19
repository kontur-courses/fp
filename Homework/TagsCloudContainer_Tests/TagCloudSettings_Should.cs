using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Console;

namespace TagsCloudContainer_Tests
{
    public class TagCloudSettings_Should
    {
        private readonly TagCloudSettings sut;

        [Test, TestCaseSource(nameof(TestSource))]
        public void ReturnFailResult_WhenIncorrectSetting(string arg)
        {
            TagCloudSettings.Parse(new []{arg}).IsSuccess.Should().BeFalse();
        }

        private static IEnumerable<string> TestSource()
        {
            yield return "-c blck";
            yield return "-f Times New Ruman";
            yield return "-s 0";
            yield return "-h 0";
            yield return "-w 0";
        }
    }
}