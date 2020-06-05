using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;
using EFCoreSample.Controls.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreSample.Controls.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandRepository _repo;
        // private readonly ISerialWrite _serialWrite;
        private readonly IActuatorRepository _actuatorRepo;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISerialCancellation _serialCancellation;

        public CommandService(ICommandRepository repo, IActuatorRepository actuatorRepo, IServiceScopeFactory serviceScopeFactory, ISerialCancellation serialCancellation)
        {
            _repo = repo;
            _actuatorRepo = actuatorRepo;
            _serviceScopeFactory = serviceScopeFactory;
            _serialCancellation = serialCancellation;
        }

        public Task<IEnumerable<Command>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<Command> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Command> Create(Guid actuatorId, CommandValue commandValue)
        {
            commandValue.IssueDate = DateTime.Now;
            // Add to database
            var result = _repo.Add(actuatorId, commandValue);
            // Start task to serial write to arduino
            StartWriteTask(actuatorId, result);
            return result;
        }

        public Task<Command> Update(Guid id, Guid actuatorId, CommandValue commandValue)
        {
            _serialCancellation.RemoveSource(id);
            var result =  _repo.Update(id, commandValue);
            // Start task to serial write to arduino
            StartWriteTask(actuatorId, result);
            return result;
        }

        public Task<bool> Delete(Guid id)
        {
            _serialCancellation.RemoveSource(id);
            return _repo.Delete(id);
        }

        private async void StartWriteTask(Guid actuatorId, Task<Command> command)
        {
            var result = await command;
            
            if (result.Value.ExecuteDate <= DateTime.Now)
            {
                Console.WriteLine("ExecuteDate passed: " + result.Value.ExecuteDate);
                Console.WriteLine("Now: " + DateTime.Now);
                return;
            }
            
            var actuator = await _actuatorRepo.Get(actuatorId);
            var writeSource = new CancellationTokenSource();
            _serialCancellation.AddSource(result.Id, writeSource);

            //fire and forget, will confirm in database upon completion
            _ = Task.Run(async () =>
                {
                    Console.WriteLine("Run Task");
                    Command written;
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var serialWrite = scope.ServiceProvider.GetRequiredService<ISerialWrite>();
                        written = await serialWrite.Write(actuator, result, writeSource);
                    }

                    return written;
                },
                writeSource.Token).ContinueWith(async task =>
            {
                var newResult = await task;
                if (newResult == null) return;
                if (newResult.Value.Repeat == TimeSpan.Zero)
                {
                    _serialCancellation.RemoveSource(newResult.Id);
                }
                else
                {
                    Console.WriteLine("old: " + newResult.Value.ExecuteDate);
                    Console.WriteLine("repeat: " + newResult.Value.Repeat);
                    var newTime = newResult.Value.ExecuteDate + newResult.Value.Repeat;
                    Console.WriteLine("new: " + newTime);
                    newResult.Value.ExecuteDate += newResult.Value.Repeat;
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<ICommandService>();
                        _ = service.Update(newResult.Id, actuatorId ,newResult.Value);
                    }
                    Console.WriteLine("Repeated");
                }
            });
        }
    }
}