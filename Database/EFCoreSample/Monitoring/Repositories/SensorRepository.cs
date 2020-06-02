using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Monitoring.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly AppDatabaseContext _db;
        private readonly IMonitoringConverter _converter;

        public SensorRepository(AppDatabaseContext db, IMonitoringConverter converter)
        {
            _db = db;
            _converter = converter;
        }

        public async Task<IEnumerable<Sensor>> GetAll()
        {
            var dataModels = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .ToListAsync();
            dataModels.ForEach(sensor => sensor.Readings = sensor.Readings
                .OrderByDescending(reading => reading.TimeStamp)
                .ToList());
            return dataModels.Select(_converter.ToSensorDomainModel);
        }
        
        public async Task<IEnumerable<Sensor>> GetAllLatest(int num)
        {
            var dataModels = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .ToListAsync();
            dataModels.ForEach(sensor => sensor.Readings = sensor.Readings
                .OrderByDescending(reading => reading.TimeStamp)
                .Take(num)
                .ToList());
            return dataModels.Select(_converter.ToSensorDomainModel);
        }

        public async Task<IEnumerable<Sensor>> GetAllRange(DateTime start, DateTime end)
        {
            var dataModels = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .ToListAsync();
            dataModels.ForEach(sensor => sensor.Readings = sensor.Readings
                .Where(reading => reading.TimeStamp >= start && reading.TimeStamp <= end)
                .OrderByDescending(reading => reading.TimeStamp)
                .ToList());
            return dataModels.Select(_converter.ToSensorDomainModel);
        }

        public async Task<Sensor> Get(Guid id)
        {
            var dataModel = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .FirstOrDefaultAsync(sensor => sensor.Id == id);
            if (dataModel != null)
                dataModel.Readings = dataModel.Readings
                    .OrderByDescending(reading => reading.TimeStamp)
                    .ToList();
            return dataModel == null ? null : _converter.ToSensorDomainModel(dataModel);
        }
        
        public async Task<Sensor> GetLatest(Guid id, int num)
        {
            var dataModel = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .FirstOrDefaultAsync(sensor => sensor.Id == id);
            if (dataModel != null)
                dataModel.Readings = dataModel.Readings
                    .OrderByDescending(reading => reading.TimeStamp)
                    .Take(num)
                    .ToList();
            return dataModel == null ? null : _converter.ToSensorDomainModel(dataModel);
        }
        
        public async Task<Sensor> GetRange(Guid id, DateTime start, DateTime end)
        {
            var dataModel = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .FirstOrDefaultAsync(sensor => sensor.Id == id);
            if (dataModel != null)
                dataModel.Readings = dataModel.Readings
                    .Where(reading => reading.TimeStamp >= start && reading.TimeStamp <= end)
                    .OrderByDescending(reading => reading.TimeStamp)
                    .ToList();
            return dataModel == null ? null : _converter.ToSensorDomainModel(dataModel);
        }

        public async Task<Sensor> Add(SensorValue value)
        {
            if (value == null) return null;
            var sensor = new Sensor(Guid.NewGuid(), value);
            
            await _db.Sensors.AddAsync(_converter.ToSensorDataModel(sensor));
            await _db.SaveChangesAsync();
            return await Get(sensor.Id);
        }

        public async Task<Sensor> Update(Guid id, SensorValue value)
        {
            var toBeUpdated = await _db.Sensors
                .Include(sensor => sensor.Readings)
                .FirstOrDefaultAsync(sensor => sensor.Id == id);
            if (toBeUpdated == null) return null;
            
            toBeUpdated.Position = value.Position;
            toBeUpdated.Type = value.Type;

            await _db.SaveChangesAsync();
            return _converter.ToSensorDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Sensors
                .Include(e => e.Readings)
                .FirstOrDefaultAsync(sensor => sensor.Id == id);

            if (toBeDeleted == null) return false;
            
            _db.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}