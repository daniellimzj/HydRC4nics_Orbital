using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;
using EFCoreSample.Controls.Repositories;

namespace EFCoreSample.Controls.Services
{
    public class ActuatorService : IActuatorService
    {
        private readonly IActuatorRepository _repo;

        public ActuatorService(IActuatorRepository repo)
        {
            _repo = repo;
        }
        
        public Task<IEnumerable<Actuator>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<IEnumerable<Actuator>> GetAllActive()
        {
            return _repo.GetAllActive();
        }
        
        public Task<IEnumerable<Actuator>> GetAllLatest(int num)
        {
            return _repo.GetAllLatest(num);
        }

        public Task<IEnumerable<Actuator>> GetAllRange(DateTime start, DateTime end)
        {
            return _repo.GetAllRange(start, end);
        }

        public Task<Actuator> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Actuator> GetActiveById(Guid id)
        {
            return _repo.GetActive(id);
        }
        
        public Task<Actuator> GetLatestById(Guid id, int num)
        {
            return _repo.GetLatest(id, num);
        }
        
        public Task<Actuator> GetRangeById(Guid id, DateTime start, DateTime end)
        {
            return _repo.GetRange(id, start, end);
        }

        public Task<Actuator> Create(ActuatorValue value)
        {
            return _repo.Add(value);
        }

        public Task<Actuator> Update(Guid id, ActuatorValue value)
        {
            return _repo.Update(id, value);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}