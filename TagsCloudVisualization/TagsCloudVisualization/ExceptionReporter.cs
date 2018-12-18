using System;

namespace TagsCloudVisualization
{
    public class ExceptionReporter
    {
        public ExceptionReporter(Action<string> process)
        {
            this.process = process;
        }
        public Action<string> process;
    }
}
