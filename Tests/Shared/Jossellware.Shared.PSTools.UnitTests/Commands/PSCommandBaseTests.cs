namespace Jossellware.Shared.PSTools.UnitTests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Jossellware.Shared.PSTools.Errors;
    using Jossellware.Shared.PSTools.UnitTests.TestUtils;
    using Jossellware.Shared.Threading;
    using Moq;
    using Xunit;

    public class PSCommandBaseTests : IDisposable
    {
        #region Constructor test cases
        public static readonly object[][] Ctor_NullArguments_TestCases = new object[][]
        {
            new object[]
            {
                default(IErrorFactory),
                new Mock<ICancellationTokenSourceFactory>().Object,
                "errorFactory"
            },
            new object[]
            {
                new Mock<IErrorFactory>().Object,
                default(ICancellationTokenSourceFactory),
                "ctsFactory"
            },
            new object[]
            {
                default(IErrorFactory),
                default(ICancellationTokenSourceFactory),
                "errorFactory"
            }
        };
        #endregion

        private CancellationTokenSource cts;
        private Mock<IErrorFactory> mockErrorFactory;
        private Mock<ICancellationTokenSourceFactory> mockCtsFactory;
        private PSTestCommand sut;
        private bool disposed;

        [Theory]
        [MemberData(nameof(Ctor_NullArguments_TestCases))]
        public void Ctor_NullArguments_ThrowsException(IErrorFactory errorFactory, ICancellationTokenSourceFactory ctsFactory, string expectedParamName)
        {
            new Func<PSTestCommand>(() => new PSTestCommand(errorFactory, ctsFactory, false))
                .Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be(expectedParamName);
        }

        [Fact]
        public void Ctor_ValidArguments_SetsExpectedParameters()
        {
            // Act
            this.DoSetup();

            // Assert
            this.sut.Should().NotBeNull();
            this.sut.PublicErrorFactory.Should().Be(this.mockErrorFactory.Object);
            this.sut.PublicCtsFactory.Should().Be(this.mockCtsFactory.Object);
            this.sut.PublicCancellationToken.Should().Be(this.cts.Token);
        }

        [Fact]
        public void Dispose_NotYetDisposed_CallsImplementationAndDisposesSource()
        {
            // Arrange
            this.DoSetup();

            // Act
            this.sut.Dispose();

            // Assert
            this.sut.VerifyMethodCalls(nameof(IDisposable.Dispose), 1);
            this.sut.PublicCancellationToken.IsCancellationRequested.Should().BeTrue();
            this.cts.Invoking(x => x.Cancel())
                .Should().ThrowExactly<ObjectDisposedException>();
            this.mockCtsFactory.As<IDisposable>().Verify(x => x.Dispose(), Times.Never);
        }

        [Fact]
        public void Dispose_AlreadyDisposed_DoesNotCallImplementation()
        {
            // Arrange
            this.DoSetup();

            // Act
            this.sut.Dispose();
            this.sut.Dispose();

            // Assert
            this.sut.VerifyMethodCalls(nameof(IDisposable.Dispose), 1);
            this.mockCtsFactory.As<IDisposable>().Verify(x => x.Dispose(), Times.Never);
        }

        [Fact]
        public void Dispose_ShouldDisposeDependenciesSetToTrue_DisposesDependencies()
        {
            // Arrange
            this.DoSetup(true);
            this.mockCtsFactory.As<IDisposable>().Setup(x => x.Dispose());

            // Act
            this.sut.Dispose();

            // Assert
            this.mockCtsFactory.As<IDisposable>().Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void BeginProcessing_NotCancelled_CallsImplementation()
        {
            // Arrange
            this.DoSetup();

            // Act
            this.sut.PublicBeginProcessing();

            // Assert
            this.sut.VerifyMethodCallsExcept("BeginProcessingImplementation", 1, 0);
        }

        [Fact]
        public void BeginProcessing_Cancelled_DoesNotCallImplementation()
        {
            // Arrange
            this.DoSetup();
            this.cts.Cancel();

            // Act
            this.sut.PublicBeginProcessing();

            // Assert
            this.sut.VerifyMethodCalls("BeginProcessingImplementation", 0);
            this.sut.VerifyNoMethodCalls();
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.sut != null)
                {
                    // The command disposes the cts for us
                    this.sut.Dispose();
                }
                else if (this.cts != null)
                {
                    this.cts.Cancel();
                    this.cts.Dispose();
                }

                this.disposed = true;
            }
        }

        private void DoSetup(bool shouldDisposeDependencies = false)
        {
            this.cts = new CancellationTokenSource();
            this.mockErrorFactory = new Mock<IErrorFactory>(MockBehavior.Strict);

            this.mockCtsFactory = new Mock<ICancellationTokenSourceFactory>(MockBehavior.Strict);
            this.mockCtsFactory.Setup(x => x.BuildSource())
                .Returns(this.cts);
            this.mockCtsFactory.As<IDisposable>().Setup(x => x.Dispose());

            this.sut = new PSTestCommand(this.mockErrorFactory.Object, this.mockCtsFactory.Object, shouldDisposeDependencies);
        }
    }
}
