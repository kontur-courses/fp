using System.Collections.Generic;
using System.Linq;
using Functional;

namespace TagCloudVisualization
{
    /// <summary>
    ///     Composes one or more drawers;
    /// </summary>
    public class CompositeDrawer
    {
        private readonly List<IWordDrawer> drawers;

        /// <summary>
        ///     Aggregates drawers so that the earliest drawer is being used to draw word;
        /// </summary>
        public CompositeDrawer(IEnumerable<IWordDrawer> drawers)
        {
            this.drawers = drawers.ToList();
        }

        public Result<IWordDrawer> GetDrawer(WordInfo wordInfo)
        {
            return drawers.GetFirst(d => d.Check(wordInfo));
        }
    }
}
