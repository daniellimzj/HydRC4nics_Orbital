namespace EFCoreSample.Monitoring.Domain
{
    public class SensorValue
    {
        public SensorValue(string position, string type)
        {
            Position = position;
            Type = type;
        }

        public string Position { get; }
        public string Type { get; }
    }
}