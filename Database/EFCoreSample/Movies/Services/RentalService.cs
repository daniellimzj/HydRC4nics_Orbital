using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;
using EFCoreSample.Movies.Repositories;

namespace EFCoreSample.Movies.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repo;

        public RentalService(IRentalRepository repo)
        {
            _repo = repo;
        }
        
        public Task<IEnumerable<MovieRental>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<MovieRental> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<MovieRental> Create(MovieRentalValue value)
        {
            return _repo.Add(value);
        }

        public Task<MovieRental> Update(Guid id, MovieRentalValue value)
        {
            return _repo.Update(id, value);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}