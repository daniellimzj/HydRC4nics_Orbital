using System;
using System.Threading;
using System.Threading.Tasks;
using EFCoreSample.Controls.Repositories;
using EFCoreSample.Monitoring.Domain;

namespace EFCoreSample.Controls.Domain
{
    public class SerialWrite : ISerialWrite
    {
        private readonly IPort _port;
        private readonly ICommandRepository _repo;

        public SerialWrite(IPort port, ICommandRepository repo)
        {
            _port = port;
            _repo = repo;
        }

        public async Task<Command> Write(Actuator actuator, Command command, CancellationTokenSource writeSource)
        {
            Console.WriteLine("Start timer: " + (command.Value.ExecuteDate - DateTime.Now));
            await Task.Delay(command.Value.ExecuteDate - DateTime.Now);

            // Check writeSource token
            if (writeSource.Token.IsCancellationRequested)
            {
                Console.WriteLine("Command was cancelled");
                writeSource.Dispose();
                return command;
            }
            
            var result = await _repo.Complete(command.Id);
            if (result == null)
            {
                Console.WriteLine("Command not found");
                return null;
            }

            // _port.Serial.WriteLine(actuator.Value.Position + ":" + command.Value.Value);
            Console.WriteLine("Completed: " + result.Id);
            return result;

        }
    }
}