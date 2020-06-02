using EFCoreSample.Monitoring.Data;
using EFCoreSample.Monitoring.Requests;
using EFCoreSample.Monitoring.Responses;

namespace EFCoreSample.Monitoring.Domain
{
    public interface IMonitoringConverter
    {
        ReadingResponse ToReadingResponse(Reading reading);
        ReadingValue ToReadingValue(ReadingRequest readingRequest);
        Reading ToReadingDomainModel(ReadingDataModel readingDataModel);
        ReadingDataModel ToReadingDataModel(Reading reading);
        SensorResponse ToSensorResponse(Sensor sensor);
        SensorValue ToSensorValue(SensorRequest sensorRequest);
        Sensor ToSensorDomainModel(SensorDataModel sensorDataModel);
        SensorDataModel ToSensorDataModel(Sensor sensor);
    }
}