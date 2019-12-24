using System;
using System.IO;
using edu.stanford.nlp.tagger.maxent;

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
                throw new TagCloudException("Error while loading a tagger model" +
                                           " Unable to open " + model + "\n" + 
                                            "Please, add this MaxentTagger model file to make program" +
                                           " be able to parse words.\n" +
                                           "Look for information about this: \"edu.stanford.nlp.tagger.maxent\"");
            }
        }

        public abstract bool IsDull(string s);
    }
}