using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Libraries.Requests
{
    public class BookRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
    }
}