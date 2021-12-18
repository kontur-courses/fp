using ResultMonad;

namespace TagsCloudVisualization
{
    public readonly struct Tag
    {
        public readonly string Word;
        public readonly float Weight;

        private Tag(string word, float weight)
        {
            Word = word;
            Weight = weight;
        }

        public static Result<Tag> Create(string word, float weight)
        {
            return Result.Ok()
                .Validate(() => weight is > 0 and <= 1, $"{nameof(weight)} expected be in (0, 1], but actual {weight}")
                .Validate(() => !string.IsNullOrEmpty(word), $"{nameof(word)} is not correct, actual = {word}")
                .Then(new Tag(word, weight));
        }

        public override string ToString() => $"{nameof(Word)} = {Word}, {nameof(Weight)} = {Weight}";
    }
}