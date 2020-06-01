using System;
using System.Collections.Generic;

namespace EFCoreSample.Libraries.Data
{
    public class LibraryDataModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<BookDataModel> Books { get; set; } = new List<BookDataModel>();
    }
}