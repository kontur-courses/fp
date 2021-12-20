using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsCloudController : Controller
    {
        private readonly ITagsCloudVisualizer tagsCloudVisualizer;

        public TagsCloudController(ITagsCloudVisualizer tagsCloudVisualizer)
            => this.tagsCloudVisualizer = tagsCloudVisualizer;

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] string text)
        {
            var result = tagsCloudVisualizer.GenerateImage(new SimpleTextReader(text))
                .Then(image =>
                {
                    using var ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    image.Dispose();

                    return File(ms.ToArray(), "image/png");
                });

            return result.IsSuccess
                ? result.GetValueOrThrow()
                : StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }
    }
}