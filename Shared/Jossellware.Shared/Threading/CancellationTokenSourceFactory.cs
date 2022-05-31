﻿namespace Jossellware.Shared.Threading
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class CancellationTokenSourceFactory : ICancellationTokenSourceFactory, IDisposable
    {
        private bool disposed;
        private readonly ConcurrentQueue<CancellationTokenSource> sources;

        public CancellationTokenSourceFactory()
        {
            this.sources = new ConcurrentQueue<CancellationTokenSource>();
        }

        public CancellationTokenSource BuildSource(TimeSpan timeout)
        {
            var source = new CancellationTokenSource(timeout);
            this.sources.Enqueue(source);
            return source;
        }

        public CancellationTokenSource BuildSource()
        {
            var source = new CancellationTokenSource();
            this.sources.Enqueue(source);
            return source;
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.sources?.Any() == true)
                {
                    while (this.sources.TryDequeue(out var result))
                    {
                        result.Cancel();
                        result.Dispose();
                    }
                }

                this.disposed = true;
            }
        }
    }
}