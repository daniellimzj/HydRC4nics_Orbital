using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace EFCoreSample.Monitoring.Domain
{
    public interface ISerialRead
    {
        List<Guid> Sequence { get; set; }
        SerialPort Start();
        SerialPort Stop();
    }
}