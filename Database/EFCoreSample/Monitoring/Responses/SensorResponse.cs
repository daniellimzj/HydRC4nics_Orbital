using System;
using System.Collections.Generic;

namespace EFCoreSample.Monitoring.Responses
{
    public class SensorResponse
    {
        public SensorResponse(Guid id, string position, string type, List<ReadingResponse> readings)
        {
            Id = id;
            Position = position;
            Type = type;
            Readings = readings;
        }

        public Guid Id { get; }
        public string Position { get; }
        public string Type { get; }
        public List<ReadingResponse> Readings { get; }
    }
}