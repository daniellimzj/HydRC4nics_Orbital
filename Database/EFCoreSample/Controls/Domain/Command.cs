using System;

namespace EFCoreSample.Controls.Domain
{
    public class Command 
    {
        public Command(Guid id, CommandValue value)
        {
            Id = id;
            Value = value;
        }
        
        public Guid Id { get; }
        public CommandValue Value { get; }
    }
}