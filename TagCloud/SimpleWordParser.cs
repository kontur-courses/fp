using System;
using System.IO;
using NHunspell;
using ResultOf;

namespace TagCloud
{
    public class SimpleWordParser : IWordParser
    {
        private readonly Hunspell hunspell;
        public SimpleWordParser()
        {
            hunspell = new Hunspell("en_US.aff", "en_US.dic");
        }

        public bool IsValidWord(string word)
        {
            return hunspell.Spell(word);
        }
    }
}