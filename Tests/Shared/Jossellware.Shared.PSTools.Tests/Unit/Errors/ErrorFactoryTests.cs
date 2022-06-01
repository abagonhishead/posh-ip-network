namespace Jossellware.Shared.PSTools.Tests.Unit.Errors
{
    using System.Management.Automation;
    using FluentAssertions;
    using Jossellware.Shared.PSTools.Errors;
    using Xunit;

    public class ErrorFactoryTests
    {
        private readonly IErrorFactory sut;

        public ErrorFactoryTests()
        {
            this.sut = new ErrorFactory();
        }

        [Fact]
        public void BuildError_NullException_ThrowsException()
        {
            // Assert
            this.sut.Invoking(x => x.BuildError(default(Exception), "fullyQualiifiedErrorId", ErrorCategory.InvalidArgument, new object()))
                .Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("exception");
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(default(string))]
        public void BuildError_NullOrWhitespaceErrorId_ThrowsException(string errorId)
        {
            // Assert
            this.sut.Invoking(x => x.BuildError(new Exception(), errorId, ErrorCategory.InvalidArgument, new object()))
                .Should().ThrowExactly<ArgumentException>()
                .Where(x => x.ParamName == "fullyQualifiedErrorId")
                .And.Message.Should().StartWith("Cannot be null or whitespace.");
        }

        [Fact]
        public void BuildError_ValidParameters_ReturnsErrorRecord()
        {
            // Arrange
            var exception = new Exception("I am exceptional");
            var targetObject = new
            {
                SomeProperty = "SomeValue"
            };

            // Act
            var result = this.sut.BuildError(exception, "FullyQualifiedErrorId", ErrorCategory.WriteError, targetObject);

            // Assert
            result.Should().NotBeNull();
            result.Exception.Should().Be(exception);
            result.TargetObject.Should().Be(targetObject);
            result.CategoryInfo.Should().NotBeNull();
            result.CategoryInfo.Category.Should().Be(ErrorCategory.WriteError);
            result.FullyQualifiedErrorId.Should().Be("FullyQualifiedErrorId");
            result.ErrorDetails.Should().BeNull();
            result.InvocationInfo.Should().BeNull();
            result.PipelineIterationInfo.Should().BeEmpty();
            result.ScriptStackTrace.Should().BeNull();
        }
    }
}