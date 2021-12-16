using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.Tests.FluentAssertionsExtensions
{
    public class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
    {
        protected override string Identifier => "result";
        public ResultAssertions(Result<T> subject) : base(subject) { }

        public AndConstraint<ResultAssertions<T>> BeFailed(string error = null)
        {
            using (new AssertionScope(nameof(Subject.IsSuccess)))
                Subject.IsSuccess.Should().BeFalse();

            return error == null
                ? new AndConstraint<ResultAssertions<T>>(this)
                : AssertError(error);
        }

        private AndConstraint<ResultAssertions<T>> AssertError(string error)
        {
            using (new AssertionScope(nameof(Subject.Error)))
                Subject.Error.Should().Be(error);

            return new AndConstraint<ResultAssertions<T>>(this);
        }

        public AndConstraint<ResultAssertions<T>> BeOk(T value = default)
        {
            using (new AssertionScope(nameof(Subject.IsSuccess)))
                Subject.IsSuccess.Should().BeTrue();

            return Equals(value, default)
                ? new AndConstraint<ResultAssertions<T>>(this)
                : AssertValue(value);
        }

        private AndConstraint<ResultAssertions<T>> AssertValue(T value)
        {
            using (new AssertionScope(nameof(Subject.GetValueOrThrow)))
                Subject.GetValueOrThrow().Should().BeEquivalentTo(value);

            return new AndConstraint<ResultAssertions<T>>(this);
        }
    }
}