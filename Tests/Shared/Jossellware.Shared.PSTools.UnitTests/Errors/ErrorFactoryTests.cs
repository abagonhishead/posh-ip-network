namespace Jossellware.Shared.PSTools.UnitTests.Errors
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
                .And.Message.Should().Be("Cannot be null or whitespace.");
        }
    }
}