using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.TestFrameworks;
using System.CodeDom;
using System.Runtime.CompilerServices;

namespace TagCloud.Tests
{
    [TestFixture]
    public class MorphologicalFilterTests
    {
        [Test]
        [TestCase(PartSpeech.Noun)]
        [TestCase(PartSpeech.Verb)]
        [TestCase(PartSpeech.Noun | PartSpeech.Verb)]
        [TestCase(PartSpeech.Adjective)]
        [UseReporter(typeof(DiffReporter), typeof(NUnitReporter))]
        public void Filter_SuccessPath(PartSpeech partSpeech)
        {
            var settings = new WordExtractionOptions() { PartsSpeech = partSpeech };
            var filter = new MorphologicalFilter(settings);
            var words = File.ReadLines(PathFinderHelper.GetPathToFile("words.txt")).ToArray();

            var filtered = filter.Filter(words);

            using (ApprovalTests.Namers.ApprovalResults.ForScenario(partSpeech))
            {
                Approvals.Verify(string.Join("\n", filtered));
            }
        }
    }
}
