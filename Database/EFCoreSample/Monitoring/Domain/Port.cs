using System.IO.Ports;

namespace EFCoreSample.Monitoring.Domain
{
    public class Port : IPort
    {
        public SerialPort Serial { get; }

        public Port()
        {
            Serial = new SerialPort();
        }

        public string SetPortName(string com)
        {
            Serial.PortName = com;
            return com;
        }

        public int SetBaudRate(int baudRate)
        {
            Serial.BaudRate = baudRate;
            return baudRate;
        }

        public int SetDataBits(int dataBits)
        {
            Serial.DataBits = dataBits;
            return dataBits;
        }

        public Parity SetParity(Parity parity)
        {
            Serial.Parity = Parity.None;
            return Parity.None;
        }

        public StopBits SetStopBits(StopBits stopBits)
        {
            Serial.StopBits = StopBits.One;
            return StopBits.One;
        }
    }
}