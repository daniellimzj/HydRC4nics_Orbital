using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;
using EFCoreSample.Libraries.Repositories;

namespace EFCoreSample.Libraries.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Book>> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<Book> GetById(Guid id)
        {
            return _repo.Get(id);
        }

        public Task<Book> Create(Guid libraryId, BookValue bookValue)
        {
            return _repo.Add(libraryId, bookValue);
        }

        public Task<Book> Update(Guid id, BookValue bookValue)
        {
            return _repo.Update(id, bookValue);
        }

        public Task<bool> Delete(Guid id)
        {
            return _repo.Delete(id);
        }
    }
}