using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudForm.Common;

namespace TagsCloudForm.Common
{
    public interface ITextReader
    {
        Result<IEnumerable<string>> ReadLines(string fileName);
    }
}
