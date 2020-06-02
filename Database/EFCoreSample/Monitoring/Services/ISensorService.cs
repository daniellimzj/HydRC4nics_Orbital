using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;

namespace EFCoreSample.Monitoring.Services
{
    public interface ISensorService
    {
        Task<IEnumerable<Sensor>> GetAll();
        
        Task<IEnumerable<Sensor>> GetAllLatest(int num);
        
        Task<IEnumerable<Sensor>> GetAllRange(DateTime start, DateTime end);

        Task<Sensor> GetById(Guid id);
        
        Task<Sensor> GetLatestById(Guid id, int num);
        
        Task<Sensor> GetRangeById(Guid id, DateTime start, DateTime end);

        Task<Sensor> Create(SensorValue value);

        Task<Sensor> Update(Guid id, SensorValue value);

        Task<bool> Delete(Guid id);
    }
}