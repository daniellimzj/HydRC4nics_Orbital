using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;
using EFCoreSample.Movies.Repositories;

namespace EFCoreSample.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repo;

        public MovieService(IMovieRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Movie>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<Movie> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Movie> Create(Guid libraryId, MovieValue movieValue)
        {
            return _repo.Add(libraryId, movieValue);
        }

        public Task<Movie> Update(Guid id, MovieValue movieValue)
        {
            return _repo.Update(id, movieValue);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}