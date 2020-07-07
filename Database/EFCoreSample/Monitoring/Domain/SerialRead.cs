using System;
using System.Collections.Generic;
using System.IO.Ports;
using EFCoreSample.Controls.Domain;
using EFCoreSample.Monitoring.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreSample.Monitoring.Domain
{
    public class SerialRead : ISerialRead
    {
        private readonly IPort _port;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public List<Guid> Sequence { get; set; }
        

        public SerialRead(IReadingRepository repo, IPort port, IServiceScopeFactory serviceScopeFactory)
        {
            _port = port;
            _serviceScopeFactory = serviceScopeFactory;
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
            var data = new List<string>(_port.Serial.ReadLine().Split(','));
            // Show all the incoming data in the port's buffer
            data.ForEach(i => Console.Write("{0}\t", i));
            // Value1, Units1, Value2, Units2, ...
            var timeStamp = DateTime.Now;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                for (var i = 0; i < Sequence.Count; i++)
                {
                    if (data.Count < 2 * (i + 1)) break; // In case not all serial data was read
                    var canConvert = int.TryParse(data[2 * i], out var val);
                    if (!canConvert)
                    {
                        Console.WriteLine("Serial data error: value is not a number");
                        break;
                    }
                    var reading = new ReadingValue(val, data[2 * i + 1], timeStamp);
                    var repo = scope.ServiceProvider.GetRequiredService<IReadingRepository>();
                    _ = await repo.Add(Sequence[i], reading);
                }
            }
        }
    }
}