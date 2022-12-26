namespace Tests;

public class ResultTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CorrectDivision()
    {
        var a = 2;
        var b = 6;
        var result = Result.Of(() => b / a, "Деление на 0");
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(3);
    }
    [Test]
    public void DivisionByZero_Error()
    {
        var a = 0;
        var b = 6;
        var result = Result.Of(() => b / a, "Деление на 0");
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Деление на 0");
    }
}