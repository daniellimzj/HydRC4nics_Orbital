using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Monitoring.Repositories
{
    public class ReadingRepository : IReadingRepository
    {
        private readonly AppDatabaseContext _db;
        private readonly IMonitoringConverter _converter;

        public ReadingRepository(AppDatabaseContext db, IMonitoringConverter converter)
        {
            _db = db;
            _converter = converter;
        }
        
        public async Task<IEnumerable<Reading>> GetAll()
        {
            return (await _db.Readings.ToListAsync())
                .Select(_converter.ToReadingDomainModel);
        }

        public async Task<Reading> Get(Guid id)
        {
            var dataModel = await _db.Readings.FindAsync(id);
            return dataModel == null ? null : _converter.ToReadingDomainModel(dataModel);
        }

        public async Task<Reading> Add(Guid sensorId, ReadingValue value)
        {
            var toBeAdded = new Reading(Guid.NewGuid(), value);
            var dataModel = _converter.ToReadingDataModel(toBeAdded);
            await _db.Readings.AddAsync(dataModel);
            
            var sensor = await _db.Sensors.FindAsync(sensorId);
            if (sensor == null) return null;
            sensor.Readings.Add(dataModel);

            await _db.SaveChangesAsync();
            return await Get(toBeAdded.Id);
        }

        public async Task<Reading> Update(Guid readingId, ReadingValue value)
        {
            var toBeUpdated = await _db.Readings.FindAsync(readingId);
            if (toBeUpdated == null) return null; 
            
            toBeUpdated.Value = value.Value;
            toBeUpdated.Units = value.Units;
            toBeUpdated.TimeStamp = value.TimeStamp;

            await _db.SaveChangesAsync();
            return _converter.ToReadingDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Readings.FindAsync(id);
            if (toBeDeleted == null) return false;
            
            _db.Readings.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}