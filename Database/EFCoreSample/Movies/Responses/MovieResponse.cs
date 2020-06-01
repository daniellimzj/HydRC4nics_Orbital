using System;

namespace EFCoreSample.Movies.Responses
{
    public class MovieResponse
    {
        public MovieResponse(Guid id, string title, string directorName, DateTime releaseDate)
        {
            Id = id;
            Title = title;
            DirectorName = directorName;
            ReleaseDate = releaseDate;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string DirectorName { get; }
        public DateTime ReleaseDate { get; }
    }
}