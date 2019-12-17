using System.Collections.Generic;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.IServices
{
    public interface ITagCollectionFactory
    {
        Result<List<Tag>> Create(ImageSettings imageSettings, string path);
    }
}