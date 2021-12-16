// using System;
// using System.Reflection;
// using FluentAssertions;
// using NUnit.Framework;
// using TagsCloudApp.Parsers;
// using TagsCloudContainer.Preprocessing;
//
// namespace TagsCloud.Tests
// {
//     public class EnumParserTests
//     {
//         private EnumParser parser;
//
//         [SetUp]
//         public void SetUp()
//         {
//             parser = new EnumParser();
//         }
//
//         [TestCase("A", ExpectedResult = SpeechPart.A)]
//         [TestCase("ADV", ExpectedResult = SpeechPart.ADV)]
//         public SpeechPart Parse_ReturnCorrectValue(string value) =>
//             parser.TryParse<SpeechPart>(value).Value;
//
//         [TestCase("all")]
//         [TestCase("ALL")]
//         public void Parse_IgnoreCase(string value)
//         {
//             parser.TryParse<MemberTypes>(value)
//                 .Value.Should().Be(MemberTypes.All);
//         }
//
//         [Test]
//         public void Parse_ReturnFailResult_WithIncorrectValue()
//         {
//             parser.TryParse<SpeechPart>("QWE")
//                 .Exception.Should().BeOfType<ApplicationException>();
//         }
//     }
// }