namespace Jossellware.Shared.Tests.Unit.Threading
{
    using FluentAssertions;
    using Jossellware.Shared.Threading;
    using Xunit;

    public class CancellationTokenSourceFactoryTests
    {
        private readonly CancellationTokenSourceFactory sut;

        public CancellationTokenSourceFactoryTests()
        {
            this.sut = new CancellationTokenSourceFactory();
        }

        [Fact]
        public void BuildSource_NoParameters_ReturnsSourceWithNoTimeout()
        {
            // Act
            var result = this.sut.BuildSource();

            // Assert
            result.Should().NotBeNull();
            result.IsCancellationRequested.Should().BeFalse();
        }

        [Fact]
        public void BuildSource_TimeSpan_ReturnsSourceWithTimeout()
        {
            // Act
            var result = this.sut.BuildSource(TimeSpan.FromSeconds(0));

            // Assert
            result.Should().NotBeNull();
            result.IsCancellationRequested.Should().BeTrue();
        }

        /* TODO: test for Dispose */
    }
}