using System;
using System.Collections.Generic;

namespace EFCoreSample.Movies.Domain
{
    public class MovieRental
    {
        public MovieRental(Guid id, MovieRentalValue value)
        {
            Id = id;
            Value = value;
        }
        public Guid Id { get; }
        public MovieRentalValue Value { get; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}