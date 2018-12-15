using System.Collections.Generic;
using Result;
using static Result.Result;

namespace TagCloudVisualization
{
    public abstract class AbstractSpiralGenerator
    {
        private protected IEnumerator<Point> Enumerator;

        private protected Point Center { get; private set; }

        public Result<AbstractSpiralGenerator> Begin(Point center)
        {
            if (Enumerator != null)
                return Fail<AbstractSpiralGenerator>("Begin method must be called only once");

            Center = center;
            Enumerator = GetEnumerator();
            Enumerator.MoveNext();
            return Ok(this);
        }

        private protected abstract IEnumerator<Point> GetEnumerator();

        public Result<Point> Next()
        {
            if (Enumerator == null)
                return Fail<Point>("Begin method must be called before usage of this method");
            Enumerator.MoveNext();
            return Ok(Enumerator.Current);
        }
    }
}
