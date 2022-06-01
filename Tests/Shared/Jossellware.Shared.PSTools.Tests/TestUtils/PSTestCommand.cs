namespace Jossellware.Shared.PSTools.Tests.TestUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            this.methodCalls = new Dictionary<string, int>();
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

        public void ShouldHaveMethodCall(string methodName, int expectedCallCount)
        {
            this.GetMethodCallCount(methodName).Should().Be(expectedCallCount);
        }

        public void ShouldNotHaveMethodCall(string methodName)
        {
            this.GetMethodCallCount(methodName).Should().Be(0);
        }

        public void ShouldOnlyHaveMethodCalls(int expectedCallCount, params string[] methodNames)
        {
            this.methodCalls.Should().OnlyContain(
                x => methodNames.Contains(x.Key, StringComparer.Ordinal) &&
                    x.Value == expectedCallCount);
        }

        public void ShouldHaveNoMethodCalls()
        {
            this.methodCalls.Should().BeEmpty();
        }

        protected override void Dispose(bool disposing = false)
        {
            this.UpdateMethodCallCounter(nameof(this.Dispose));

            base.Dispose(disposing);
        }

        protected override void BeginProcessingImplementation()
        {
            this.UpdateMethodCallCounter(nameof(this.BeginProcessingImplementation));

            base.BeginProcessingImplementation();
        }

        protected override void EndProcessingImplementation()
        {
            this.UpdateMethodCallCounter(nameof(this.EndProcessingImplementation));

            base.EndProcessingImplementation();
        }

        protected override void PreprocessRecord()
        {
            this.UpdateMethodCallCounter(nameof(this.PreprocessRecord));

            base.PreprocessRecord();
        }

        protected override void ProcessRecordImplementation()
        {
            this.UpdateMethodCallCounter(nameof(this.ProcessRecordImplementation));
        }

        protected override void StopProcessingImplementation()
        {
            this.UpdateMethodCallCounter(nameof(this.StopProcessingImplementation));
            base.StopProcessingImplementation();
        }

        private void UpdateMethodCallCounter(string methodName)
        {
            var calls = 1;
            if (this.methodCalls.TryGetValue(methodName, out var val))
            {
                calls += val;
            }

            this.methodCalls[methodName] = calls;
        }

        private int GetMethodCallCount(string methodName)
        {
            return this.methodCalls.TryGetValue(methodName, out var val) ? val : 0;
        }
    }
}
