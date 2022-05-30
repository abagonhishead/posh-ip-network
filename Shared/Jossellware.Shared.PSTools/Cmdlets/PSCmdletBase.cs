namespace Jossellware.Shared.PSTools.Cmdlets
{
    using System;
    using System.Management.Automation;

    public abstract class PSCmdletBase<TReturn> : PSCmdlet
    {
        private ErrorRecord Error;
        private TReturn Output;

        public override string GetResourceString(string baseName, string resourceId)
        {
            return base.GetResourceString(baseName, resourceId);
        }

        protected abstract TReturn ProcessRecordImplementation();

        protected virtual void BeginProcessingImplementation()
        {
        }

        protected virtual void EndProcessingImplementation()
        {
        }

        protected virtual void StopProcessingImplementation()
        {
        }

        protected sealed override void BeginProcessing()
        {
            this.BeginProcessingImplementation();
            base.BeginProcessing();
        }

        protected sealed override void ProcessRecord()
        {
            if (this.Error == null)
            {
                this.Output = this.ProcessRecordImplementation();
            }

            base.ProcessRecord();
        }

        protected sealed override void EndProcessing()
        {
            this.EndProcessingImplementation();

            if (this.Error == null)
            {
                this.WriteObject(this.Output);
            }
            else
            {
                this.WriteError(this.Error);
            }

            base.EndProcessing();
        }

        protected sealed override void StopProcessing()
        {
            this.StopProcessingImplementation();
            base.StopProcessing();
        }

        protected void SetError(Exception exception, string errorId, ErrorCategory? category = null, object targetObject = null, Exception inner = null)
        {
            var record = new ErrorRecord(exception, errorId, category ?? ErrorCategory.NotSpecified, targetObject);
            if (inner != null)
            {
                record = new ErrorRecord(record, inner);
            }

            this.SetError(record);
        }

        protected void SetError(string message, string errorId, Exception inner, ErrorCategory? category = null, object targetObject = null)
        {
            var exception = new CmdletInvocationException(message);
            var record = new ErrorRecord(exception, errorId, category ?? ErrorCategory.NotSpecified, targetObject);
            if (inner != null)
            {
                record = new ErrorRecord(record, inner);
            }

            this.SetError(record);
        }

        protected void SetError(ErrorRecord errorRecord)
        {
            this.Error = errorRecord;
        }

        protected bool IsParameterSetNamed(string parameterSetName)
        {
            return string.Equals(parameterSetName, this.ParameterSetName);
        }
    }
}
