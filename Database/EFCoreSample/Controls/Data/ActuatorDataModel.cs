using System;
using System.Collections.Generic;

namespace EFCoreSample.Controls.Data
{
    public class ActuatorDataModel
    {
        public Guid Id { get; set; }
        public string Position { get; set; }
        public string Type { get; set; }
        public List<CommandDataModel> Commands { get; set; } = new List<CommandDataModel>();
    }
}