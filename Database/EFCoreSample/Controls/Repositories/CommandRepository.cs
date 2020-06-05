using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Controls.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDatabaseContext _db;
        private readonly IControlsConverter _converter;

        public CommandRepository(AppDatabaseContext db, IControlsConverter converter)
        {
            _db = db;
            _converter = converter;
        }
        
        public async Task<IEnumerable<Command>> GetAll()
        {
            return (await _db.Commands.ToListAsync())
                .Select(_converter.ToCommandDomainModel);
        }

        public async Task<Command> Get(Guid id)
        {
            var dataModel = await _db.Commands.FindAsync(id);
            return dataModel == null ? null : _converter.ToCommandDomainModel(dataModel);
        }

        public async Task<Command> Add(Guid actuatorId, CommandValue value)
        {
            var toBeAdded = new Command(Guid.NewGuid(), value);
            var dataModel = _converter.ToCommandDataModel(toBeAdded);
            await _db.Commands.AddAsync(dataModel);
            
            var actuator = await _db.Actuators.FindAsync(actuatorId);
            if (actuator == null) return null;
            actuator.Commands.Add(dataModel);

            await _db.SaveChangesAsync();
            return await Get(toBeAdded.Id);
        }

        public async Task<Command> Update(Guid commandId, CommandValue value)
        {
            var toBeUpdated = await _db.Commands.FindAsync(commandId);
            if (toBeUpdated == null) return null;

            toBeUpdated.Value = value.Value;
            toBeUpdated.Units = value.Units;
            toBeUpdated.Issuer = value.Issuer;
            toBeUpdated.Purpose = value.Purpose;
            toBeUpdated.IssueDate = value.IssueDate;
            toBeUpdated.ExecuteDate = value.ExecuteDate;
            toBeUpdated.Repeat = value.Repeat;
            toBeUpdated.Completed = false;

            await _db.SaveChangesAsync();
            return _converter.ToCommandDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Commands.FindAsync(id);
            if (toBeDeleted == null) return false;
            
            _db.Commands.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Command> Complete(Guid commandId)
        {
            Console.WriteLine("Completing");
            var toBeUpdated = await _db.Commands.FindAsync(commandId);
            if (toBeUpdated == null) return null;

            toBeUpdated.Completed = true;

            await _db.SaveChangesAsync();
            return _converter.ToCommandDomainModel(toBeUpdated);
        }
    }
}