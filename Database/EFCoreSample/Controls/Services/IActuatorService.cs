using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;

namespace EFCoreSample.Controls.Services
{
    public interface IActuatorService
    {
        Task<IEnumerable<Actuator>> GetAll();

        Task<IEnumerable<Actuator>> GetAllActive();
        
        Task<IEnumerable<Actuator>> GetAllLatest(int num);
        
        Task<IEnumerable<Actuator>> GetAllRange(DateTime start, DateTime end);
        
        Task<Actuator> GetById(Guid id);

        Task<Actuator> GetActiveById(Guid id);
        
        Task<Actuator> GetLatestById(Guid id, int num);
        
        Task<Actuator> GetRangeById(Guid id, DateTime start, DateTime end);
        
        Task<Actuator> Create(ActuatorValue value);

        Task<Actuator> Update(Guid id, ActuatorValue value);

        Task<bool> Delete(Guid id);
    }
}