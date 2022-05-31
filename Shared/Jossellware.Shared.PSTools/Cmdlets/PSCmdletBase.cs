namespace Jossellware.Shared.PSTools.Cmdlets
{
    using System;
    using System.Management.Automation;
    using System.Threading;
    using Jossellware.Shared.PSTools.Errors;
    using Jossellware.Shared.Threading;

    [CmdletBinding()]
    public abstract class PSCmdletBase : PSCmdlet, IDisposable
    {
        private readonly ICancellationTokenSourceFactory ctsFactory;

        private CancellationTokenSource cts;
        private bool disposed;

        protected IErrorFactory ErrorFactory { get; set; }

        protected CancellationToken CancellationToken => this.cts.Token;

        public PSCmdletBase()
            : this(new ErrorFactory(), new CancellationTokenSourceFactory())
        {
        }

        public PSCmdletBase(IErrorFactory errorFactory, ICancellationTokenSourceFactory ctsFactory)
        {
            this.ErrorFactory = errorFactory;
            this.ctsFactory = ctsFactory;

            this.cts = this.ctsFactory.BuildSource();
        }

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
                this.cts.Cancel();
                this.cts.Dispose();
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
