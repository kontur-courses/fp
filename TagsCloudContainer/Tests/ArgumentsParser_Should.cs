using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.Tests
{
    public class ArgumentsParser_Should
    {
        [Test]
        [TestCase("White", false, TestName = "OnKnownColor")]
        [TestCase("abc", true, TestName = "OnUnknownColor")]
        public void ParseColor(string colorName, bool shouldThrow)
        {
            void Action() => ArgumentsParser.ParseColor(colorName).GetValueOrThrow();
            CheckForException(Action, shouldThrow);
        }

        [Test]
        [TestCase("White", false, TestName = "OnKnownBrushColor")]
        [TestCase("abc", true, TestName = "OnUnknownBrushColor")]
        public void ParseBrush(string colorName, bool shouldThrow)
        {
            void Action() => ArgumentsParser.ParseBrush(colorName).GetValueOrThrow();
            CheckForException(Action, shouldThrow);
        }

        [Test]
        [TestCase("Arial", false, TestName = "OnKnownFont")]
        [TestCase("abc", true, TestName = "OnUnknownFont")]
        public void ParseFont(string fontName, bool shouldThrow)
        {
            void Action() => ArgumentsParser.ParseFont(fontName, 1).GetValueOrThrow();
            CheckForException(Action, shouldThrow);
        }
        
        [Test]
        [TestCase("png", false, TestName = "OnKnownFormat")]
        [TestCase("abc", true, TestName = "OnUnknownFormat")]
        public void ParseImageFormat(string format, bool shouldThrow)
        {
            void Action() => ArgumentsParser.ParseImageFormat(format).GetValueOrThrow();
            CheckForException(Action, shouldThrow);
        }

        private static void CheckForException(Action action, bool shouldThrow)
        {
            if (shouldThrow)
                action.ShouldThrow<InvalidOperationException>();
            else
                action.ShouldNotThrow<InvalidOperationException>();
        }
    }
}