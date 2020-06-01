using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;

namespace EFCoreSample.Libraries.Repositories
{
    public interface ILibraryRepository
    {
       Task<IEnumerable<Library>> GetAll();

        Task<Library> Get(Guid id);

        Task<Library> Add(LibraryValue value);

        Task<Library> Update(Guid id, LibraryValue value);

        Task<bool> Delete(Guid id);
    }
}