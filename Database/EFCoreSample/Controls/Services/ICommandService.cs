using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;

namespace EFCoreSample.Controls.Services
{
    public interface ICommandService
    {
        Task<IEnumerable<Command>> GetAll();
        Task<Command> GetById(Guid id);
        Task<Command> Create(Guid actuatorId, CommandValue commandValue);
        Task<Command> Update(Guid id, Guid actuatorId, CommandValue commandValue);
        Task<bool> Delete(Guid id);
    }
}