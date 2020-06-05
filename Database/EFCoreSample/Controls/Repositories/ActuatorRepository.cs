using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Controls.Repositories
{
    public class ActuatorRepository : IActuatorRepository
    {
        private readonly AppDatabaseContext _db;
        private readonly IControlsConverter _converter;

        public ActuatorRepository(AppDatabaseContext db, IControlsConverter converter)
        {
            _db = db;
            _converter = converter;
        }

        public async Task<IEnumerable<Actuator>> GetAll()
        {
            var dataModels = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .ToListAsync();
            dataModels.ForEach(actuator => actuator.Commands = actuator.Commands
                .OrderByDescending(command => command.ExecuteDate)
                .ToList());
            return dataModels.Select(_converter.ToActuatorDomainModel);
        }

        public async Task<IEnumerable<Actuator>> GetAllActive()
        {
            var dataModels = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .ToListAsync();
            dataModels.ForEach(actuator => actuator.Commands = actuator.Commands
                .Where(command => command.Completed == false)
                .OrderByDescending(command => command.ExecuteDate)
                .ToList());
            dataModels = dataModels.Where(actuator => actuator.Commands.Any()).ToList();
            return dataModels.Select(_converter.ToActuatorDomainModel);
        }
        public async Task<IEnumerable<Actuator>> GetAllLatest(int num)
        {
            var dataModels = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .ToListAsync();
            dataModels.ForEach(actuator => actuator.Commands = actuator.Commands
                .OrderByDescending(command => command.ExecuteDate)
                .Take(num)
                .ToList());
            return dataModels.Select(_converter.ToActuatorDomainModel);
        }

        public async Task<IEnumerable<Actuator>> GetAllRange(DateTime start, DateTime end)
        {
            var dataModels = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .ToListAsync();
            dataModels.ForEach(actuator => actuator.Commands = actuator.Commands
                .Where(reading => reading.ExecuteDate >= start && reading.ExecuteDate <= end)
                .OrderByDescending(reading => reading.ExecuteDate)
                .ToList());
            return dataModels.Select(_converter.ToActuatorDomainModel);
        }
        

        public async Task<Actuator> Get(Guid id)
        {
            var dataModel = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .FirstOrDefaultAsync(actuator => actuator.Id == id);
            if (dataModel != null)
                dataModel.Commands = dataModel.Commands
                    .OrderByDescending(command => command.ExecuteDate)
                    .ToList();
            return dataModel == null ? null : _converter.ToActuatorDomainModel(dataModel);
        }

        public async Task<Actuator> GetActive(Guid id)
        {
            var dataModel = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .FirstOrDefaultAsync(actuator => actuator.Id == id);
            if (dataModel != null)
                dataModel.Commands = dataModel.Commands
                    .Where(command => command.Completed == false)
                    .OrderByDescending(command => command.ExecuteDate)
                    .ToList();
            return dataModel == null ? null : _converter.ToActuatorDomainModel(dataModel);
        }
        
        public async Task<Actuator> GetLatest(Guid id, int num)
        {
            var dataModel = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .FirstOrDefaultAsync(actuator => actuator.Id == id);
            if (dataModel != null)
                dataModel.Commands = dataModel.Commands
                    .OrderByDescending(command => command.ExecuteDate)
                    .Take(num)
                    .ToList();
            return dataModel == null ? null : _converter.ToActuatorDomainModel(dataModel);
        }
        
        public async Task<Actuator> GetRange(Guid id, DateTime start, DateTime end)
        {
            var dataModel = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .FirstOrDefaultAsync(actuator => actuator.Id == id);
            if (dataModel != null)
                dataModel.Commands = dataModel.Commands
                    .Where(command => command.ExecuteDate >= start && command.ExecuteDate <= end)
                    .OrderByDescending(command => command.ExecuteDate)
                    .ToList();
            return dataModel == null ? null : _converter.ToActuatorDomainModel(dataModel);
        }

        public async Task<Actuator> Add(ActuatorValue value)
        {
            if (value == null) return null;
            var actuator = new Actuator(Guid.NewGuid(), value);

            await _db.Actuators.AddAsync(_converter.ToActuatorDataModel(actuator));
            await _db.SaveChangesAsync();
            return await Get(actuator.Id);
        }

        public async Task<Actuator> Update(Guid id, ActuatorValue value)
        {
            var toBeUpdated = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .FirstOrDefaultAsync(actuator => actuator.Id == id);
            if (toBeUpdated == null) return null;

            toBeUpdated.Position = value.Position;
            toBeUpdated.Type = value.Type;

            await _db.SaveChangesAsync();
            return _converter.ToActuatorDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Actuators
                .Include(actuator => actuator.Commands)
                .FirstOrDefaultAsync(actuator => actuator.Id == id);

            if (toBeDeleted == null) return false;

            _db.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}