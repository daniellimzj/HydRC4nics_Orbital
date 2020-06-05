using System;
using System.Collections.Concurrent;
using System.Threading;

namespace EFCoreSample.Controls.Domain
{
    public class SerialCancellation : ISerialCancellation
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _sources;

        public SerialCancellation()
        {
            _sources = new ConcurrentDictionary<Guid, CancellationTokenSource>();
        }

        public void AddSource(Guid id, CancellationTokenSource source)
        {
            var added = false;
            while (!added)
            {
                added = _sources.TryAdd(id, source);
                if (!added) RemoveSource(id);
            }
        }

        public void RemoveSource(Guid id)
        {
            var exists = _sources.TryGetValue(id, out var source);
            if ( exists && !source.Token.IsCancellationRequested) source.Cancel();
            var removed = _sources.TryRemove(id, out source);
            if (removed) source.Dispose();
        }
    }
}