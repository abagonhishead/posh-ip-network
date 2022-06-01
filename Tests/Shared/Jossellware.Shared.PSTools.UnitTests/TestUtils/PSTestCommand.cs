namespace Jossellware.Shared.PSTools.UnitTests.TestUtils
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Jossellware.Shared.PSTools.Commands;
    using Jossellware.Shared.PSTools.Errors;
    using Jossellware.Shared.Threading;

    public class PSTestCommand : PSCommandBase
    {
        private readonly IDictionary<string, int> methodCalls;

        public IErrorFactory PublicErrorFactory => this.ErrorFactory;

        public ICancellationTokenSourceFactory PublicCtsFactory => this.CtsFactory;

        public CancellationToken PublicCancellationToken => this.CancellationToken;

        public PSTestCommand()
            : base()
        {
            throw new InvalidOperationException("This is a test command that requires dependencies to be passed into the constructor.");
        }

        public PSTestCommand(IErrorFactory errorFactory, ICancellationTokenSourceFactory ctsFactory, bool shouldDisposeDependencies) 
            : base(errorFactory, ctsFactory, shouldDisposeDependencies)
        {
            this.methodCalls = new Dictionary<string, int>
            {
                { nameof(this.BeginProcessingImplementation), 0 },
                { nameof(this.Dispose), 0 },
                { nameof(this.EndProcessingImplementation), 0 },
                { nameof(this.PreprocessRecord), 0 },
                { nameof(this.ProcessRecordImplementation), 0 },
                { nameof(this.StopProcessingImplementation), 0 },
            };
        }

        public void PublicBeginProcessing()
        {
            base.BeginProcessing();
        }

        public void PublicProcessRecord()
        {
            base.ProcessRecord();
        }

        public void PublicEndProcessing()
        {
            base.EndProcessing();
        }

        public void PublicStopProcessing()
        {
            base.StopProcessing();
        }

        public void VerifyMethodCalls(string methodName, int expectedCalls)
        {
            this.methodCalls[methodName].Should().Be(expectedCalls);
        }

        public void VerifyMethodCallsExcept(string exceptMethodName, int exceptMethodCalls, int remainingMethodCalls)
        {
            foreach (var methodCall in this.methodCalls)
            {
                if (string.Equals(methodCall.Key, exceptMethodName, StringComparison.Ordinal))
                {
                    methodCall.Value.Should().Be(exceptMethodCalls);
                }
                else
                {
                    methodCall.Value.Should().Be(remainingMethodCalls);
                }
            }
        }

        public void VerifyNoMethodCalls()
        {
            this.methodCalls.Should().OnlyContain(x => x.Value == 0);
        }

        protected override void BeginProcessingImplementation()
        {
            this.methodCalls[nameof(this.BeginProcessingImplementation)]++;
            base.BeginProcessingImplementation();
        }

        protected override void Dispose(bool disposing = false)
        {
            this.methodCalls[nameof(this.Dispose)]++;
            base.Dispose(disposing);
        }

        protected override void EndProcessingImplementation()
        {
            this.methodCalls[nameof(this.EndProcessingImplementation)]++;
            base.EndProcessingImplementation();
        }

        protected override void PreprocessRecord()
        {
            this.methodCalls[nameof(this.PreprocessRecord)]++;
            base.PreprocessRecord();
        }

        protected override void ProcessRecordImplementation()
        {
            this.methodCalls[nameof(this.ProcessRecordImplementation)]++;
        }

        protected override void StopProcessingImplementation()
        {
            this.methodCalls[nameof(this.StopProcessingImplementation)]++;
            base.StopProcessingImplementation();
        }
    }
}
