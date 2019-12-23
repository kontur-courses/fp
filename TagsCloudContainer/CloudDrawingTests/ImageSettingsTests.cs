using System.Drawing;
using CloudDrawing;
using FluentAssertions;
using NUnit.Framework;

namespace CloudDrawingTests
{
    public class ImageSettingsTests
    {
        [TestCase("", TestName = "Empty string")]
        [TestCase(null, TestName = "Null")]
        [TestCase("*", TestName = "\"*\"")]
        [TestCase("Block", TestName = "Block")]
        [TestCase("black", TestName = "black")]
        public void GetImageSettingsError_WhenColorIsNotInSystem(string color)
        {
            ImageSettings.GetImageSettings(color, 0, 0).IsSuccess.Should().BeFalse();
        }
        
        [Test, Combinatorial]
        public void GetImageSettingsError_WhenSizeNotPositive(
            [Values (-100, -1, 0)]int width, 
            [Values (-100, -1, 0)]int height)
        {
            ImageSettings.GetImageSettings("YellowGreen", width, height).IsSuccess.Should().BeFalse();
        }
        
        [Test, Combinatorial]
        public void GetImageSettingsError_WhenWidthOrHeightNotPositive(
            [Values (-100, -1, 0)]int dimension1, 
            [Values (-100, -1, 0, 1, 100)]int dimension2)
        {
            ImageSettings.GetImageSettings("RosyBrown", dimension1, dimension2).IsSuccess.Should().BeFalse();
            ImageSettings.GetImageSettings("RosyBrown", dimension2, dimension1).IsSuccess.Should().BeFalse();
        }
        
        [Test, Combinatorial]
        public void GetImageSettingsSuccessfully_WhenGoodSize(
            [Values (1, 100, 1000)]int width, 
            [Values (1, 100, 1000)]int height)
        {
            ImageSettings.GetImageSettings("HotPink", width, height).IsSuccess.Should().BeTrue();
        }
        
        [TestCase("Black", TestName = "Black")]
        [TestCase("RosyBrown", TestName = "RosyBrown")]
        [TestCase("YellowGreen", TestName = "YellowGreen")]
        [TestCase("MediumPurple", TestName = "MediumPurple")]
        [TestCase("HotPink", TestName = "HotPink")]
        public void GetImageSettingsSuccessfully_WhenGoodColor(string color)
        {
            ImageSettings.GetImageSettings(color, 1, 1).IsSuccess.Should().BeTrue();
        }
        
        
    }
}