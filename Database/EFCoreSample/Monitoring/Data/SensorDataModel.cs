using System;
using System.Collections.Generic;

namespace EFCoreSample.Monitoring.Data
{
    public class SensorDataModel
    {
        public Guid Id { get; set; }
        public string Position { get; set; }
        public string Type { get; set; }
        public List<ReadingDataModel> Readings { get; set; } = new List<ReadingDataModel>();
    }
}