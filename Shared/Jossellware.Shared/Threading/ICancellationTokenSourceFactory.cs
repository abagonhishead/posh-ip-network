namespace Jossellware.Shared.Threading
{
    using System;
    using System.Threading;

    public interface ICancellationTokenSourceFactory
    {
        CancellationTokenSource BuildSource();
        CancellationTokenSource BuildSource(TimeSpan timeout);
    }
}