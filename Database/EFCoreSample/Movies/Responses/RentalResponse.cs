using System;
using System.Collections.Generic;
using EFCoreSample.Movies.Domain;

namespace EFCoreSample.Movies.Responses
{
    public class RentalResponse
    {
        public RentalResponse(Guid id, string name, string address, List<Movie> movies)
        {
            Id = id;
            Name = name;
            Address = address;
            Movies = movies;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Address { get; }
        public List<Movie> Movies { get; }
    }
}