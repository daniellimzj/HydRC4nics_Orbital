using System;

namespace EFCoreSample.Movies.Domain
{
    public class MovieValue
    {
        public MovieValue(string title, string directorName, DateTime releaseDate)
        {
            Title = title;
            DirectorName = directorName;
            ReleaseDate = releaseDate;
        }

        public string Title { get; }
        public string DirectorName { get; }
        public DateTime ReleaseDate { get; }
    }
}