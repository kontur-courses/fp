using System;
using System.IO;
using edu.stanford.nlp.tagger.maxent;
using java.lang;

namespace TagsCloudContainer
{
    public abstract class DullWordEliminator : IDullWordsEliminator
    {
        protected readonly string model = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory,
            "english-bidirectional-distsim.tagger"});
        protected readonly MaxentTagger tagger;

        public DullWordEliminator()
        {
            try
            {
                tagger = new MaxentTagger(model);
            }
            catch
            {
                throw new System.Exception("Error while loading a tagger model" +
                                           " (probably missing model file) ---> java.io.IOException:" +
                                           " Unable to open " + model +
                                           " as class path, filename or URL");
            }
        }

        public abstract bool IsDull(string s);
    }
}