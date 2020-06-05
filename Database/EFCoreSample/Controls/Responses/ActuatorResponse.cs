using System;
using System.Collections.Generic;

namespace EFCoreSample.Controls.Responses
{
    public class ActuatorResponse
    {
        public ActuatorResponse(Guid id, string position, string type, List<CommandResponse> commands)
        {
            Id = id;
            Position = position;
            Type = type;
            Commands = commands;
        }

        public Guid Id { get; }
        public string Position { get; }
        public string Type { get; }
        public List<CommandResponse> Commands { get; }
    }
}