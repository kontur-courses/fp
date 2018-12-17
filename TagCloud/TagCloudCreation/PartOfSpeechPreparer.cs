using System;
using System.Collections.Generic;
using System.IO;
using edu.stanford.nlp.tagger.maxent;
using Functional;

namespace TagCloudCreation
{
    public abstract class PartOfSpeechPreparer : IWordPreparer
    {
        private protected readonly MaxentTagger Tagger;

        private protected readonly Dictionary<string, PartOfSpeech> TagMapping = new Dictionary<string, PartOfSpeech>
        {
            ["VB"] = PartOfSpeech.Verb, ["VBD"] = PartOfSpeech.Verb, ["VBG"] = PartOfSpeech.Verb,
            ["VBN"] = PartOfSpeech.Verb, ["VBZ"] = PartOfSpeech.Verb, ["JJ"] = PartOfSpeech.Adjective,
            ["JJR"] = PartOfSpeech.Adjective, ["JJS"] = PartOfSpeech.Adjective, ["MD"] = PartOfSpeech.Modal,
            ["NN"] = PartOfSpeech.Noun, ["NNS"] = PartOfSpeech.Noun, ["NNP"] = PartOfSpeech.Noun,
            ["NNPS"] = PartOfSpeech.Noun, ["UH"] = PartOfSpeech.Interjection, ["I" + "N"] = PartOfSpeech.Preposition
        };

        protected PartOfSpeechPreparer()
        {
            var s = Path.DirectorySeparatorChar;
            Tagger = new MaxentTagger($"..{s}..{s}..{s}TagCloudCreation{s}english-bidirectional-distsim.tagger");
        }

        public abstract Result<Maybe<string>> PrepareWord(string word, TagCloudCreationOptions options);

        private protected Result<Maybe<string>> ProcessWordByTag(
            string word,
            Func<PartOfSpeech, string, Maybe<string>> wordTagActor)
        {
            return Tagger.GetTag(word)
                         .Then(tag => TagMapping.Get(tag))
                         .OnFail(error =>
                         {
                             if (word == "for")
                                 Console.WriteLine($@"Setting untagged for word {word} from err {error}");
                             return PartOfSpeech.Untagged;
                         })
                         .Then(tag => wordTagActor(tag, word));
        }
    }
}
