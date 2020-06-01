using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;

namespace EFCoreSample.Movies.Repositories
{
    public interface IRentalRepository
    {
       Task<IEnumerable<MovieRental>> GetAll();

        Task<MovieRental> Get(Guid id);

        Task<MovieRental> Add(MovieRentalValue value);
        
        Task<MovieRental> Update(Guid id, MovieRentalValue value);

        Task<bool> Delete(Guid id);
    }
}