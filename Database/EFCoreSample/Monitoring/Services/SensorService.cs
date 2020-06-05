using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;
using EFCoreSample.Monitoring.Repositories;

namespace EFCoreSample.Monitoring.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _repo;

        public SensorService(ISensorRepository repo)
        {
            _repo = repo;
        }
        
        public Task<IEnumerable<Sensor>> GetAll()
        {
            return _repo.GetAll();
        }
        
        public Task<IEnumerable<Sensor>> GetAllLatest(int num)
        {
            return _repo.GetAllLatest(num);
        }

        public Task<IEnumerable<Sensor>> GetAllRange(DateTime start, DateTime end)
        {
            return _repo.GetAllRange(start, end);
        }
        
        public Task<Sensor> GetById(Guid id)
        {
            return _repo.Get(id);
        }
        
        public Task<Sensor> GetLatestById(Guid id, int num)
        {
            return _repo.GetLatest(id, num);
        }
        
        public Task<Sensor> GetRangeById(Guid id, DateTime start, DateTime end)
        {
            return _repo.GetRange(id, start, end);
        }

        public Task<Sensor> Create(SensorValue value)
        {
            return _repo.Add(value);
        }

        public Task<Sensor> Update(Guid id, SensorValue value)
        {
            return _repo.Update(id, value);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}