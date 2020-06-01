using System;

namespace EFCoreSample.Libraries.Data
{
    public class BookDataModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishDate { get; set; }
    }
}