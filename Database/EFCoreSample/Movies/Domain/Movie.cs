using System;

namespace EFCoreSample.Movies.Domain
{
    public class Movie 
    {
        public Movie(Guid id, MovieValue value)
        {
            Id = id;
            Value = value;
        }
        
        public Guid Id { get; }
        public MovieValue Value { get; }
    }
}