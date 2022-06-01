namespace Jossellware.Shared.PSTools.Commands
{
    using System;
    using System.Management.Automation;
    using System.Threading;
    using Jossellware.Shared.PSTools.Errors;
    using Jossellware.Shared.Threading;

    [CmdletBinding()]
    public abstract class PSCommandBase : PSCmdlet, IDisposable
    {
        private CancellationTokenSource cts;
        private bool disposed;
        private bool shouldDisposeDependencies;

        protected IErrorFactory ErrorFactory { get; private set; }

        protected ICancellationTokenSourceFactory CtsFactory { get; private set; }

        protected CancellationToken CancellationToken { get; private set; }

        public PSCommandBase()
            : this(new ErrorFactory(), new CancellationTokenSourceFactory(), true)
        {
        }

        public PSCommandBase(IErrorFactory errorFactory, ICancellationTokenSourceFactory ctsFactory, bool shouldDisposeDependencies = false)
        {
            this.ErrorFactory = errorFactory ?? throw new ArgumentNullException(nameof(errorFactory));
            this.CtsFactory = ctsFactory ?? throw new ArgumentNullException(nameof(ctsFactory));

            this.cts = this.CtsFactory.BuildSource();
            this.CancellationToken = this.cts.Token;
            this.shouldDisposeDependencies = shouldDisposeDependencies;
        }

        /* TODO: Override this and use a JSON provider */
        public override string GetResourceString(string baseName, string resourceId)
        {
            return base.GetResourceString(baseName, resourceId);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this.disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing = false)
        {
            if (disposing)
            {
                this.CancellationToken = new CancellationToken(true);
                this.cts.Cancel();
                this.cts.Dispose();

                if (this.shouldDisposeDependencies &&
                    this.CtsFactory != null)
                {
                    ((IDisposable)this.CtsFactory).Dispose();
                }
            }
        }

        protected abstract void ProcessRecordImplementation();

        protected virtual void BeginProcessingImplementation()
        {
        }

        protected virtual void EndProcessingImplementation()
        {
        }

        protected virtual void StopProcessingImplementation()
        {
        }

        protected override void BeginProcessing()
        {
            if (!this.CancellationToken.IsCancellationRequested)
            {
                base.BeginProcessing();

                this.BeginProcessingImplementation();
            }
        }

        protected virtual void PreprocessRecord()
        {
        }

        protected override void ProcessRecord()
        {
            if (!this.CancellationToken.IsCancellationRequested)
            {
                this.PreprocessRecord();

                base.ProcessRecord();

                this.ProcessRecordImplementation();
            }
        }

        protected override void EndProcessing()
        {
            if (!this.CancellationToken.IsCancellationRequested)
            {
                this.EndProcessingImplementation();

                base.EndProcessing();
            }
        }

        protected override void StopProcessing()
        {
            this.Stop();

            this.StopProcessingImplementation();
            base.StopProcessing();
        }

        protected bool IsParameterSetNamed(string parameterSetName)
        {
            return string.Equals(parameterSetName, this.ParameterSetName);
        }

        protected void Stop()
        {
            this.cts.Cancel();
        }
    }
}
