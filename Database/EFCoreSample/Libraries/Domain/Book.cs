using System;

namespace EFCoreSample.Libraries.Domain
{
    public class Book 
    {
        public Book(Guid id, BookValue value)
        {
            Id = id;
            Value = value;
        }
        
        public Guid Id { get; }
        public BookValue Value { get; }
    }
}