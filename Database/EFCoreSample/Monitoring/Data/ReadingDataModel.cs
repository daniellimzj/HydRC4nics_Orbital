using System;

namespace EFCoreSample.Monitoring.Data
{
    public class ReadingDataModel
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Units { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}