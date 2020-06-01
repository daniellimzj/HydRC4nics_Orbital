using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;

namespace EFCoreSample.Libraries.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAll();
        Task<Book> GetById(Guid id);
        Task<Book> Create(Guid libraryId, BookValue bookValue);
        Task<Book> Update(Guid id, BookValue bookValue);
        Task<bool> Delete(Guid id);
    }
}