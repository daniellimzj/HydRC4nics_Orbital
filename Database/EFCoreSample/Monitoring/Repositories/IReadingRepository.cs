using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;

namespace EFCoreSample.Monitoring.Repositories
{
    public interface IReadingRepository
    {
        Task<IEnumerable<Reading>> GetAll();

        Task<Reading> Get(Guid id);

        Task<Reading> Add(Guid sensorId, ReadingValue value);

        Task<Reading> Update(Guid readingId, ReadingValue value);

        Task<bool> Delete(Guid id);
    }
}