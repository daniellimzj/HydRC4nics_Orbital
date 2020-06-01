using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;

namespace EFCoreSample.Libraries.Services
{
    public interface ILibraryService
    {
        Task<IEnumerable<Library>> GetAll();

        Task<Library> GetById(Guid id);

        Task<Library> Create(LibraryValue value);

        Task<Library> Update(Guid id, LibraryValue value);

        Task<bool> Delete(Guid id);
    }
}