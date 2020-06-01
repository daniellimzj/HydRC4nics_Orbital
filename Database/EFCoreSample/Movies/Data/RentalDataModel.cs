using System;
using System.Collections.Generic;

namespace EFCoreSample.Movies.Data
{
    public class RentalDataModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<MovieDataModel> Movies { get; set; } = new List<MovieDataModel>();
    }
}