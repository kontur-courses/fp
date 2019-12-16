using System.Collections.Generic;
using System.IO;
using System.Linq;
using edu.stanford.nlp.tagger.maxent;

namespace TagsCloudVisualization.Preprocessing
{
    public class RemoveNotNounsPreprocessor : IPreprocessor
    {
        private const string Model = "english-bidirectional-distsim.tagger";
        private MaxentTagger tagger;

        public IEnumerable<string> ProcessWords(IEnumerable<string> words)
        {
            if(tagger == null)
                LoadTagger();
            return words.Where(word => IsNoun(tagger.tagString(word)));
        }

        private void LoadTagger()
        {
            try
            {
                tagger = new MaxentTagger(Model);
            }
            catch
            {
                throw new FileLoadException("Could not load preprocessing module");
            }
        }

        private static bool IsNoun(string taggedString) //https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html
        {
            var tag = taggedString.Split('_').Last();
            return tag.Contains("NN");
        }
    }
}