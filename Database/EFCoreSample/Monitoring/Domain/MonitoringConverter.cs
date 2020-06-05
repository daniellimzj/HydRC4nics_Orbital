using System.Linq;
using EFCoreSample.Monitoring.Data;
using EFCoreSample.Monitoring.Requests;
using EFCoreSample.Monitoring.Responses;

namespace EFCoreSample.Monitoring.Domain
{
    public class MonitoringConverter : IMonitoringConverter
    {
        public ReadingResponse ToReadingResponse(Reading reading)
        {
            return new ReadingResponse(reading.Id, reading.Value.Value, reading.Value.Units, reading.Value.TimeStamp);
        }

        public ReadingValue ToReadingValue(ReadingRequest readingRequest)
        {
            return new ReadingValue(readingRequest.Value, readingRequest.Units, readingRequest.TimeStamp);
        }

        public Reading ToReadingDomainModel(ReadingDataModel readingDataModel)
        {
            return new Reading(readingDataModel.Id, 
                new ReadingValue(readingDataModel.Value, readingDataModel.Units, readingDataModel.TimeStamp));
        }

        public ReadingDataModel ToReadingDataModel(Reading reading)
        {
            return new ReadingDataModel
            {
                Id = reading.Id,
                Value = reading.Value.Value,
                Units = reading.Value.Units,
                TimeStamp = reading.Value.TimeStamp
            };
        }

        public SensorResponse ToSensorResponse(Sensor sensor)
        {
            return new SensorResponse(sensor.Id, sensor.Value.Position, sensor.Value.Type,
                sensor.Readings?.Select(ToReadingResponse).ToList());
        }

        public SensorValue ToSensorValue(SensorRequest sensorRequest)
        {
            return new SensorValue(sensorRequest.Position, sensorRequest.Type);
        }

        public Sensor ToSensorDomainModel(SensorDataModel sensorDataModel)
        {
            return new Sensor(sensorDataModel.Id, new SensorValue(sensorDataModel.Position, sensorDataModel.Type))
            {
                Readings = sensorDataModel.Readings?.Select(ToReadingDomainModel).ToList()
            };
        }

        public SensorDataModel ToSensorDataModel(Sensor sensor)
        {
            return new SensorDataModel
            {
                Id = sensor.Id,
                Position = sensor.Value.Position,
                Type = sensor.Value.Type,
                Readings = sensor.Readings?.Select(ToReadingDataModel).ToList()
            };
        }
    }
}