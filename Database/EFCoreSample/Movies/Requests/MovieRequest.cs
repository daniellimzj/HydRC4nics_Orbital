using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSample.Movies.Requests
{
    public class MovieRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string DirectorName { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}