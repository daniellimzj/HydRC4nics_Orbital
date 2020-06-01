using System;

namespace EFCoreSample.Movies.Data
{
    public class MovieDataModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DirectorName { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}