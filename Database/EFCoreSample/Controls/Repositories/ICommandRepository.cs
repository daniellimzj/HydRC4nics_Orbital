using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;

namespace EFCoreSample.Controls.Repositories
{
    public interface ICommandRepository
    {
        Task<IEnumerable<Command>> GetAll();

        Task<Command> Get(Guid id);

        Task<Command> Add(Guid actuatorId, CommandValue value);

        Task<Command> Update(Guid commandId, CommandValue value);

        Task<bool> Delete(Guid id);

        Task<Command> Complete(Guid commandId);
    }
}