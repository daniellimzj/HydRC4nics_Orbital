using System;
using System.Collections.Generic;

namespace EFCoreSample.Monitoring.Domain
{
    public class Sensor
    {
        public Sensor(Guid id, SensorValue value)
        {
            Id = id;
            Value = value;
        }
        public Guid Id { get; }
        public SensorValue Value { get; }
        public List<Reading> Readings { get; set; } = new List<Reading>();
    }
}