[Flags]
public enum PartSpeech
{
    None = 0,
    Adjective = 1,
    Adverb = 1 << 1,
    PronominalAdverb = 1 << 2,
    NumeralAdjective = 1 << 3,
    PronounAdjective = 1 << 4,
    PartComposite = 1 << 5,
    Union = 1 << 6,
    Interjection = 1 << 7,
    Numeral = 1 << 8,
    Particle = 1 << 9,
    Preposition = 1 << 10,
    Noun = 1 << 11,
    PronounNoun = 1 << 12,
    Verb = 1 << 13
}
