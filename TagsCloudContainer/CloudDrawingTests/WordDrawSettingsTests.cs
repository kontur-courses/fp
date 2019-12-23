using CloudDrawing;
using FluentAssertions;
using NUnit.Framework;

namespace CloudDrawingTests
{
    public class WordDrawSettingsTests
    {
        [TestCase("", TestName = "Empty string")]
        [TestCase(null, TestName = "Null")]
        [TestCase("*", TestName = "\"*\"")]
        [TestCase("Block", TestName = "Block")]
        [TestCase("black", TestName = "black")]
        public void GetWordDrawSettingsError_WhenColorIsNotInSystem(string color)
        {
            WordDrawSettings.GetWordDrawSettings("", color, true).IsSuccess.Should().BeFalse();
        }
        
        [TestCase("Black", TestName = "Black")]
        [TestCase("Chocolate", TestName = "Chocolate")]
        [TestCase("DarkBlue", TestName = "DarkBlue")]
        [TestCase("Firebrick", TestName = "Firebrick")]
        [TestCase("LimeGreen", TestName = "LimeGreen")]
        public void GetWordDrawSettingsSuccessfully(string color)
        {
            WordDrawSettings.GetWordDrawSettings("", color, false).IsSuccess.Should().BeTrue();
        }
    }
}