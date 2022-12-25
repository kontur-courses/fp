using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ResultOf;

namespace TagsCloudContainer.Infrastructure
{
    public interface ITextReader
    {
        public string Filter { get; }

        public Result<string> ReadText(string path);
    }
}
