using System.IO.Ports;

namespace EFCoreSample.Monitoring.Domain
{
    public interface IPort
    {
        SerialPort Serial { get; }
        string SetPortName(string com);
        int SetBaudRate(int baudRate);
        int SetDataBits(int dataBits);
        Parity SetParity(Parity parity);
        StopBits SetStopBits(StopBits stopBits);
    }
}