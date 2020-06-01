using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;

namespace EFCoreSample.Movies.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(Guid id);
        Task<Movie> Create(Guid libraryId, MovieValue movieValue);
        Task<Movie> Update(Guid id, MovieValue movieValue);
        Task<bool> Delete(Guid id);
    }
}