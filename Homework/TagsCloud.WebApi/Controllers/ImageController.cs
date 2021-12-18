using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.WebApi.Services;

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
            using var image = layouterCore.GenerateImage(new SimpleTextReader(text));
            
            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            
            return File(ms.ToArray(), "image/png");
        }
    }
}