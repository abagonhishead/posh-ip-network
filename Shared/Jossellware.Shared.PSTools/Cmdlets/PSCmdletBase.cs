namespace Jossellware.Shared.PSTools.Cmdlets
{
    using System;
    using System.Management.Automation;

    public class PSCmdletBase : PSCmdlet
    {
        protected void BuildAndWriteError(Exception exception, string errorId, ErrorCategory? category = null, object targetObject = null, Exception inner = null)
        {
            var record = new ErrorRecord(exception, errorId, category ?? ErrorCategory.NotSpecified, targetObject);
            if (inner != null)
            {
                record = new ErrorRecord(record, inner);
            }

            this.WriteError(record);
        }

        protected void BuildAndWriteError(string message, string errorId, Exception inner, ErrorCategory? category = null, object targetObject = null)
        {
            var exception = new CmdletInvocationException(message);
            var record = new ErrorRecord(exception, errorId, category ?? ErrorCategory.NotSpecified, targetObject);
            if (inner != null)
            {
                record = new ErrorRecord(record, inner);
            }

            this.WriteError(record);
        }
    }
}
