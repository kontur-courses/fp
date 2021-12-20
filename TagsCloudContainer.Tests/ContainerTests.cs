using FluentAssertions;
using NUnit.Framework;
using ResultOf;
using System;
using System.Collections.Generic;
using System.Reflection;
using TagsCloudContainer;
using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Tests;

public class ContainerTests
{

    [Test]
    public void CanChangeUsableImplementations()
    {
        var args = new[] { "--assemblies", Assembly.GetExecutingAssembly().Location, "--implement-with", $"{nameof(IRunner)} {nameof(ThrowingRunner)}" };
        var result = InitializationHelper.RunWithArgs(args);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(ThrowingRunner.ExceptionText);
    }

    [Test]
    public void ShouldReturnOkResult_WhenAtLeastRequiredSettingsProvided()
    {
        var result = InitializationHelper.RunWithArgs(new[] { "--string", "тэгер тэг тэги" });

        result.IsSuccess.Should().BeTrue();
    }

    [TestCaseSource(nameof(IncorrectArgsSource))]
    public void ShouldReturnFailedResult_WhenBothDefaultInputProvided(IncorrectArg arg)
    {
        var result = InitializationHelper.RunWithArgs(arg.Args);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to run").And.Contain(arg.Why);
    }

    [Test]
    public void ShouldReturnFailedResult_WhenNoRequiredSettingsProvided()
    {
        var result = InitializationHelper.RunWithArgs(Array.Empty<string>());

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("One of the required arguments");
    }

    [TestCase("Abc", nameof(DefaultRunner))]
    [TestCase(nameof(IRunner), "Abc")]
    public void ShouldReturnFailedResult_WhenInvalidImplementationArgs(string service, string implementation)
    {
        var result = InitializationHelper.RunWithArgs(new[] { "--implement-with", $"{service} {implementation}" });

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Could not find type with name");
    }

    [Test]
    public void ShouldReturnFailedResult_WhenUnknownAssemblyIsProvided()
    {
        var result = InitializationHelper.RunWithArgs(new[] { "--assemblies", $"abc" });

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Could not load assembly");
    }

    [Test]
    public void ServiceIndex_Should_CorrectlyIndexAvailableServicesAndImplementationsFromAssemblies()
    {
        var serviceIndex = new ServiceIndex();

        serviceIndex.AddAssemblyTypes(Assembly.GetExecutingAssembly());
        var implementation = serviceIndex.GetImplementation(nameof(TotallyLegitimateService));
        var service = serviceIndex.GetService(nameof(ITotallyLegitimateService));

        implementation.IsSuccess.Should().BeTrue();
        implementation.GetValueOrThrow().Implementation.Should().Be(typeof(TotallyLegitimateService));
        service.IsSuccess.Should().BeTrue();
        service.GetValueOrThrow().Should().Be(typeof(ITotallyLegitimateService));
        implementation.GetValueOrThrow().ImplementedServices.Should().BeEquivalentTo(new[] { service.GetValueOrThrow() });
    }

    [Test]
    public void ServiceIndex_ShouldReturnFailedResults_WhenRequestedUnknown()
    {
        var serviceIndex = new ServiceIndex();

        var implementation = serviceIndex.GetImplementation(nameof(TotallyLegitimateService));
        var service = serviceIndex.GetService(nameof(ITotallyLegitimateService));

        implementation.IsSuccess.Should().BeFalse();
        implementation.Error.Should().Contain($"Could not find type with name '{nameof(TotallyLegitimateService)}'.");
        service.IsSuccess.Should().BeFalse();
        service.Error.Should().Contain($"Could not find type with name '{nameof(ITotallyLegitimateService)}'.");
    }

    private static IEnumerable<IncorrectArg> IncorrectArgsSource()
    {
        yield return new(new[] { "--files", "abc" }, "Could not find file");
        yield return new(new[] { "--string", "тэгер тэг тэги", "--files", "abc" }, "Can't use both file and string input providers");
        yield return new(new[] { "--center", "abc" }, "String abc was in incorrect format, should be two ints separated by ','");
        yield return new(new[] { "--image-format", "abc" }, "Could not convert string `abc' to type ImageFormatType");
        yield return new(new[] { "--add-parts", "abc" }, "Requested value 'abc' was not found.");
        yield return new(new[] { "--min-size", "abc" }, "Could not convert string `abc' to type Double");
        yield return new(new[] { "--color", "abc" }, "Could not convert string `abc' to type Color");
        yield return new(new[] { "--height", "abc" }, "Could not convert string `abc' to type Int32");
        yield return new(new[] { "--width", "abc" }, "Could not convert string `abc' to type Int32");
        yield return new(new[] { "--smoothing-mode", "abc" }, "Could not convert string `abc' to type SmoothingMode");
    }

    public record IncorrectArg(string[] Args, string Why);

    private class ThrowingRunner : IRunner
    {
        public const string ExceptionText = "tried to use throwing runner";
        public Result Run(params string[] args)
        {
            return Result.Fail(ExceptionText);
        }
    }

    private interface ITotallyLegitimateService : IService
    {

    }

    private class TotallyLegitimateService : ITotallyLegitimateService
    {

    }
}

