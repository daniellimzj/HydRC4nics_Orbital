using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;

namespace EFCoreSample.Monitoring.Services
{
    public interface IReadingService
    {
        Task<IEnumerable<Reading>> GetAll();
        Task<Reading> GetById(Guid id);
        Task<Reading> Create(Guid sensorId, ReadingValue readingValue);
        Task<Reading> Update(Guid id, ReadingValue readingValue);
        Task<bool> Delete(Guid id);
        SerialPort SerialStart(string com, List<Guid> sequence);
        SerialPort SerialStop();
    }
}