using System.Collections.Generic;
using System.Linq;
using DeepMorphy.Model;
using TagsCloud.Interfaces;

namespace TagsCloud.TextWorkers
{
    public class NormalFormParser : INormalFormParser
    {
        public IEnumerable<string> Normalize(IEnumerable<MorphInfo> clearMorphs)
        {
            var normalForms = clearMorphs.Select(x =>
            {
                if (x.BestTag.HasLemma == false)
                {
                    return x.Text;
                }

                return x.BestTag.Lemma;
            });

            return normalForms;
        }
    }
}