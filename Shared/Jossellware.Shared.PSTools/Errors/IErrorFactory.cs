namespace Jossellware.Shared.PSTools.Errors
{
    using System;
    using System.Management.Automation;

    public interface IErrorFactory
    {
        ErrorRecord BuildError(Exception exception, string fullyQualifiedErrorId, ErrorCategory category, object target = null);
    }
}