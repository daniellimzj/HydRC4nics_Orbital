using System;

namespace EFCoreSample.Libraries.Responses
{
    public class BookResponse
    {
        public BookResponse(Guid id, string title, string authorName, DateTime publishDate)
        {
            Id = id;
            Title = title;
            AuthorName = authorName;
            PublishDate = publishDate;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string AuthorName { get; }
        public DateTime PublishDate { get; }
    }
}