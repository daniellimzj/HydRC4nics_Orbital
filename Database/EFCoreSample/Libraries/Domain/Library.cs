using System;
using System.Collections.Generic;

namespace EFCoreSample.Libraries.Domain
{
    public class Library
    {
        public Library(Guid id, LibraryValue value)
        {
            Id = id;
            Value = value;
        }
        public Guid Id { get; }
        public LibraryValue Value { get; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}