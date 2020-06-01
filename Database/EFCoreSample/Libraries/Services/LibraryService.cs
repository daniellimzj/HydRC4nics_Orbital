using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;
using EFCoreSample.Libraries.Repositories;

namespace EFCoreSample.Libraries.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _repo;

        public LibraryService(ILibraryRepository repo)
        {
            _repo = repo;
        }
        
        public Task<IEnumerable<Library>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<Library> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Library> Create(LibraryValue value)
        {
            return _repo.Add(value);
        }

        public Task<Library> Update(Guid id, LibraryValue value)
        {
            return _repo.Update(id, value);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}