using System;
using System.Threading;

namespace EFCoreSample.Controls.Domain
{
    public interface ISerialCancellation
    {
        void AddSource(Guid id, CancellationTokenSource source);
        void RemoveSource(Guid id);
    }
}