using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ImageController : Controller
    {
        private readonly ILayouterCore layouterCore;

        public ImageController(ILayouterCore layouterCore) => this.layouterCore = layouterCore;

        [HttpGet("image")]
        public IActionResult GetImage(string text)
        {
            var res = layouterCore.GenerateImage(new SimpleTextReader(text))
                .Then(image =>
                {
                    var ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    image.Dispose();
                    return ms;
                })
                .Then(stream =>
                {
                    using var m = stream;
                    return File(m.ToArray(), "image/png");
                });

            return res.IsSuccess ? res.GetValueOrThrow() : BadRequest(res.Error);
        }
    }
}