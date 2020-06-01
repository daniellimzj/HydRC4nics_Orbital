using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;

namespace EFCoreSample.Libraries.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAll();

        Task<Book> Get(Guid id);

        Task<Book> Add(Guid libraryId, BookValue value);

        Task<Book> Update(Guid bookId, BookValue value);

        Task<bool> Delete(Guid id);
    }
}