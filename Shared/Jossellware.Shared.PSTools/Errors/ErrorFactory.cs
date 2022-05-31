namespace Jossellware.Shared.PSTools.Errors
{
    using System;
    using System.Management.Automation;

    public class ErrorFactory : IErrorFactory
    {
        public ErrorRecord BuildError(Exception exception, string fullyQualifiedErrorId, ErrorCategory category, object target)
        {
            exception = exception ?? throw new ArgumentNullException(nameof(exception));
            fullyQualifiedErrorId = string.IsNullOrWhiteSpace(fullyQualifiedErrorId) ? throw new ArgumentException("Cannot be null or whitespace.", nameof(fullyQualifiedErrorId)) : fullyQualifiedErrorId;

            return new ErrorRecord(exception, fullyQualifiedErrorId, category, target);
        }
    }
}
