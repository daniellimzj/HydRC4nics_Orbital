using System;
using System.Collections.Generic;
using EFCoreSample.Libraries.Domain;

namespace EFCoreSample.Libraries.Responses
{
    public class LibraryResponse
    {
        public LibraryResponse(Guid id, string name, string address, List<Book> books)
        {
            Id = id;
            Name = name;
            Address = address;
            Books = books;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Address { get; }
        public List<Book> Books { get; }
    }
}