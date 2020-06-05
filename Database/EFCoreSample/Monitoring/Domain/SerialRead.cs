using System;
using System.Collections.Generic;
using System.IO.Ports;
using EFCoreSample.Monitoring.Repositories;

namespace EFCoreSample.Monitoring.Domain
{
    public class SerialRead : ISerialRead
    {
        private readonly IPort _port;
        private readonly IReadingRepository _repo;
        public List<Guid> Sequence { get; set; }

        public SerialRead(IReadingRepository repo, IPort port)
        {
            _port = port;
            _repo = repo;
            Sequence = null;
        }

        /*
        // Create the serial port with basic settings
        var port = new SerialPort(Com,
            9600, Parity.None, 8, StopBits.One);
        */
        
        public SerialPort Start()
        {
            Console.WriteLine("Opening serial port");

            // Attach a method to be called when there
            // is data waiting in the port's buffer
            _port.Serial.DataReceived += port_DataReceived;

            // Begin communications
            _port.Serial.Open();

            return _port.Serial;
        }

        public SerialPort Stop()
        {
            _port.Serial.Close();

            return _port.Serial;
        }

        private async void port_DataReceived(object sender,
            SerialDataReceivedEventArgs e)
        {
            var data = new List<string>(_port.Serial.ReadExisting().Split(','));
            // Show all the incoming data in the port's buffer
            Console.WriteLine(data);
            // TimeStamp, Value1, Units1, Value2, Units2, ...
            var timeStamp = DateTime.Parse(data[0]);
            data.RemoveAt(0);

            for (var i = 0; i < Sequence.Count; i++)
            {
                if (data.Count <= 2 * i) continue; // In case not all serial data was read
                var reading = ToReadingValue(int.Parse(data[2*i]), data[2*i+1], timeStamp);
                var result = await _repo.Add(Sequence[i], reading);
            }
        }
        
        private ReadingValue ToReadingValue(int value, string units, DateTime timeStamp)
        {
            return new ReadingValue(value, units, timeStamp);
        }
    }
}