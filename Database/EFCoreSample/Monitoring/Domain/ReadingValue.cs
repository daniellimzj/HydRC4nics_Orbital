using System;

namespace EFCoreSample.Monitoring.Domain
{
    public class ReadingValue
    {
        public ReadingValue(int value, string units, DateTime timeStamp)
        {
            Value = value;
            Units = units;
            TimeStamp = timeStamp;
        }

        public int Value { get; }
        public string Units { get; }
        public DateTime TimeStamp { get; }
    }
}