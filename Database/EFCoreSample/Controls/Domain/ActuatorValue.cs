namespace EFCoreSample.Controls.Domain
{
    public class ActuatorValue
    {
        public ActuatorValue(string position, string type)
        {
            Position = position;
            Type = type;
        }

        public string Position { get; }
        public string Type { get; }
    }
}