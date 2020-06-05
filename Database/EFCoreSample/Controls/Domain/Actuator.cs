using System;
using System.Collections.Generic;

namespace EFCoreSample.Controls.Domain
{
    public class Actuator
    {
        public Actuator(Guid id, ActuatorValue value)
        {
            Id = id;
            Value = value;
        }
        public Guid Id { get; }
        public ActuatorValue Value { get; }
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}