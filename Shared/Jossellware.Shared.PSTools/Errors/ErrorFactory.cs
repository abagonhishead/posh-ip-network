namespace Jossellware.Shared.PSTools.Errors
{
    using System;
    using System.Management.Automation;

    public class ErrorFactory : IErrorFactory
    {
        public ErrorRecord BuildError(Exception exception, string fullyQualifiedErrorId, ErrorCategory category, object target)
        {
            return new ErrorRecord(exception, fullyQualifiedErrorId, category, target);
        }
    }
}
