using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;

namespace EFCoreSample.Controls.Repositories
{
    public interface IActuatorRepository
    {
        Task<IEnumerable<Actuator>> GetAll();

        Task<IEnumerable<Actuator>> GetAllActive();
        
        Task<IEnumerable<Actuator>> GetAllLatest(int num);
        
        Task<IEnumerable<Actuator>> GetAllRange(DateTime start, DateTime end);
        
        Task<Actuator> Get(Guid id);
        
        Task<Actuator> GetActive(Guid id);
        
        Task<Actuator> GetLatest(Guid id, int num);
   
        Task<Actuator> GetRange(Guid id, DateTime start, DateTime end);

        Task<Actuator> Add(ActuatorValue value);
        
        Task<Actuator> Update(Guid id, ActuatorValue value);

        Task<bool> Delete(Guid id);
    }
}