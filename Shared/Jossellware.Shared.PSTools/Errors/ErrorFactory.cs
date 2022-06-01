namespace Jossellware.Shared.PSTools.Errors
{
    using System;
    using System.Management.Automation;

    // Note that this class needs to be unit tested under .NET 6.0 (referencing Microsoft.PowerShell.SDK) or .NET Framework
    public class ErrorFactory : IErrorFactory
    {
        public ErrorRecord BuildError(Exception exception, string fullyQualifiedErrorId, ErrorCategory category, object target = null)
        {
            exception = exception ?? throw new ArgumentNullException(nameof(exception));
            fullyQualifiedErrorId = string.IsNullOrWhiteSpace(fullyQualifiedErrorId) ? throw new ArgumentException("Cannot be null or whitespace.", nameof(fullyQualifiedErrorId)) : fullyQualifiedErrorId;

            return new ErrorRecord(exception, fullyQualifiedErrorId, category, target);
        }
    }
}
