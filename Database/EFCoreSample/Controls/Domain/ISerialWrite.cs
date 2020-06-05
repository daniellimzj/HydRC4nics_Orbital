using System.Threading;
using System.Threading.Tasks;
using EFCoreSample.Controls.Repositories;

namespace EFCoreSample.Controls.Domain
{
    public interface ISerialWrite
    {
        Task<Command> Write(Actuator actuator, Command command, CancellationTokenSource writeSource);
    }
}