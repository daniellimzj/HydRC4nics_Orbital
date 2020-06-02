using System;

namespace EFCoreSample.Monitoring.Responses
{
    public class ReadingResponse
    {
        public ReadingResponse(Guid id, int value, string units, DateTime timeStamp)
        {
            Id = id;
            Value = value;
            Units = units;
            TimeStamp = timeStamp;
        }

        public Guid Id { get; }
        public int Value { get; }
        public string Units { get; }
        public DateTime TimeStamp { get; }
    }
}