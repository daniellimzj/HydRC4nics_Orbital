using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;

namespace EFCoreSample.Movies.Services
{
    public interface IRentalService
    {
        Task<IEnumerable<MovieRental>> GetAll();

        Task<MovieRental> GetById(Guid id);

        Task<MovieRental> Create(MovieRentalValue value);

        Task<MovieRental> Update(Guid id, MovieRentalValue value);

        Task<bool> Delete(Guid id);
    }
}