using System;

namespace EFCoreSample.Monitoring.Domain
{
    public class Reading 
    {
        public Reading(Guid id, ReadingValue value)
        {
            Id = id;
            Value = value;
        }
        
        public Guid Id { get; }
        public ReadingValue Value { get; }
    }
}