using System;

namespace EFCoreSample.Libraries.Domain
{
    public class BookValue
    {
        public BookValue(string title, string authorName, DateTime publishDate)
        {
            Title = title;
            AuthorName = authorName;
            PublishDate = publishDate;
        }

        public string Title { get; }
        public string AuthorName { get; }
        public DateTime PublishDate { get; }
    }
}