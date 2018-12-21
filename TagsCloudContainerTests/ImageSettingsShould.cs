using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Settings;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class ImageSettingsShould
    {


        [Test]
        public void BeCreatedFromJson()
        {
            var jsonSource =
                "{\r\n    \"Height\": 1024,\r\n    \"Width\": 1024,\r\n    \"Font\": \"Times New Roman\",\r\n    \"MaxFontSize\": 100,\r\n    \"MinFontSize\": 10,\r\n    \"BackgroundColor\": \"White\",\r\n    \"TextColor\": \"Black\",\r\n    \"RectangleColor\":  \"Red\",\r\n    \"ImageFormat\": \"Bmp\" \r\n}";

            var expected = new ImageSettings(
                new Size(1024, 1024),
                new FontFamily("Times New Roman"),
                100,
                10,
                Color.White,
                Color.Black,
                Color.Red,
                ImageFormat.Bmp);

            var actual = ImageSettings.FromJson(jsonSource);

            actual.Value.Should().BeEquivalentTo(expected);
        }
    }
}