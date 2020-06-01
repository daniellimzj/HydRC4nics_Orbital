using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;

namespace EFCoreSample.Movies.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAll();

        Task<Movie> Get(Guid id);

        Task<Movie> Add(Guid rentalId, MovieValue value);

        Task<Movie> Update(Guid bookId, MovieValue value);

        Task<bool> Delete(Guid id);
    }
}