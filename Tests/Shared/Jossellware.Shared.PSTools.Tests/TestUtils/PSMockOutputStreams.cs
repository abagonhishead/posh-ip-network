namespace Jossellware.Shared.PSTools.Tests.TestUtils
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Management.Automation;
    using Moq;

    public class PSMockOutputStreams<TOutput>
    {
        private readonly ConcurrentQueue<TOutput> output;
        private readonly ConcurrentQueue<ErrorRecord> error;
        private readonly ConcurrentQueue<string> warning;
        private readonly ConcurrentQueue<string> verbose;
        private readonly ConcurrentQueue<string> debug;
        private readonly ConcurrentQueue<InformationRecord> information;
        private readonly ConcurrentQueue<ProgressRecord> progress;

        public IReadOnlyCollection<TOutput> Output => this.output;

        public IReadOnlyCollection<ErrorRecord> Error => this.error;

        public IReadOnlyCollection<string> Warning => this.warning;

        public IReadOnlyCollection<string> Verbose => this.verbose;

        public IReadOnlyCollection<string> Debug => this.debug;

        public IReadOnlyCollection<InformationRecord> Information => this.information;

        public IReadOnlyCollection<ProgressRecord> Progress => this.progress;

        public PSMockOutputStreams()
        {
            this.output = new ConcurrentQueue<TOutput>();
            this.error = new ConcurrentQueue<ErrorRecord>();
            this.warning = new ConcurrentQueue<string>();
            this.verbose = new ConcurrentQueue<string>();
            this.debug = new ConcurrentQueue<string>();
            this.information = new ConcurrentQueue<InformationRecord>();
            this.progress = new ConcurrentQueue<ProgressRecord>();
        }

        public PSMockOutputStreams(Mock<ICommandRuntime2> runtime)
            : this()
        {
            this.SetupMockStreams(runtime);
        }

        public void SetupMockStreams(Mock<ICommandRuntime2> runtime)
        {
            runtime.Setup(x => x.WriteObject(It.IsAny<TOutput>()))
                .Callback<TOutput>(x => this.output.Enqueue(x));
            runtime.Setup(x => x.WriteError(It.IsAny<ErrorRecord>()))
                .Callback<ErrorRecord>(x => this.error.Enqueue(x));
            runtime.Setup(x => x.WriteWarning(It.IsAny<string>()))
                .Callback<string>(x => this.warning.Enqueue(x));
            runtime.Setup(x => x.WriteVerbose(It.IsAny<string>()))
                .Callback<string>(x => this.verbose.Enqueue(x));
            runtime.Setup(x => x.WriteDebug(It.IsAny<string>()))
                .Callback<string>(x => this.debug.Enqueue(x));
            runtime.Setup(x => x.WriteProgress(It.IsAny<ProgressRecord>()))
                .Callback<ProgressRecord>(x => this.progress.Enqueue(x));

            runtime.Setup(x => x.WriteInformation(It.IsAny<InformationRecord>()))
                .Callback<InformationRecord>(x => this.information.Enqueue(x));
        }
    }

    public class PSMockOutputStreams : PSMockOutputStreams<object>
    {
    }
}
