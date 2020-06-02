using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;
using EFCoreSample.Monitoring.Repositories;

namespace EFCoreSample.Monitoring.Services
{
    public class ReadingService : IReadingService
    {
        private readonly IReadingRepository _repo;
        private readonly ISerialRead _serialRead;
        private readonly IPort _port;

        public ReadingService(IReadingRepository repo, ISerialRead serialRead, IPort port)
        {
            _repo = repo;
            _serialRead = serialRead;
            _port = port;
        }

        public Task<IEnumerable<Reading>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<Reading> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Reading> Create(Guid sensorId, ReadingValue readingValue)
        {
            return _repo.Add(sensorId, readingValue);
        }

        public Task<Reading> Update(Guid id, ReadingValue readingValue)
        {
            return _repo.Update(id, readingValue);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }

        public SerialPort SerialStart(string com, List<Guid> sequence)
        {
            // Create the serial port with basic settings
            _port.SetPortName(com);
            _serialRead.Sequence = sequence;
            return _serialRead.Start();
        }

        public SerialPort SerialStop()
        {
            var port = _serialRead.Stop();
            return port;
        }
    }
}