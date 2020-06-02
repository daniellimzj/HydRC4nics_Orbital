using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;

namespace EFCoreSample.Monitoring.Repositories
{
    public interface ISensorRepository
    {
        Task<IEnumerable<Sensor>> GetAll();

        Task<IEnumerable<Sensor>> GetAllLatest(int num);
        
        Task<IEnumerable<Sensor>> GetAllRange(DateTime start, DateTime end);
        
        Task<Sensor> Get(Guid id);

        Task<Sensor> GetLatest(Guid id, int num);
   
        Task<Sensor> GetRange(Guid id, DateTime start, DateTime end);
        
        Task<Sensor> Add(SensorValue value);
        
        Task<Sensor> Update(Guid id, SensorValue value);

        Task<bool> Delete(Guid id);
    }
}