using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Data;
using EFCoreSample.Libraries.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Libraries.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly AppDatabaseContext _db;

        public LibraryRepository(AppDatabaseContext db)
        {
            _db = db;
        }
        
        public async Task<IEnumerable<Library>> GetAll()
        {
            var dataModels = await _db.Libraries
                .Include(library => library.Books)
                .ToListAsync();
            return dataModels.Select(ToLibraryDomainModel);
        }

        public async Task<Library> Get(Guid id)
        {
            var dataModel = await _db.Libraries.Include(e => e.Books)
                .FirstOrDefaultAsync(entity => entity.Id == id);
            return dataModel == null? null : ToLibraryDomainModel(dataModel);
        }

        public async Task<Library> Add(LibraryValue value)
        {
            if (value == null) return null;
            var library = new Library(Guid.NewGuid(), value);
            
            await _db.Libraries.AddAsync(ToLibraryDataModel(library));
            await _db.SaveChangesAsync();
            return await Get(library.Id);
        }

        public async Task<Library> Update(Guid id, LibraryValue value)
        {
            var toBeUpdated = await _db.Libraries.Include(e => e.Books)
                .FirstOrDefaultAsync(entity => entity.Id == id);
            if (toBeUpdated == null) return null;
            
            toBeUpdated.Name = value.Name;
            toBeUpdated.Address = value.Address;

            await _db.SaveChangesAsync();
            return ToLibraryDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Libraries.Include(e => e.Books)
                .FirstOrDefaultAsync(entity => entity.Id == id);

            if (toBeDeleted == null) return false;
            
            _db.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }

        private LibraryDataModel ToLibraryDataModel(Library library)
        {
            return new LibraryDataModel
            {
                Id = library.Id,
                Name = library.Value.Name,
                Address = library.Value.Address,
                Books = library.Books?.Select(ToBookDataModel).ToList()
            };
        }

        private Library ToLibraryDomainModel(LibraryDataModel libraryDataModel)
        {
            return new Library(libraryDataModel.Id, new LibraryValue(libraryDataModel.Name, libraryDataModel.Address))
            {
                Books = libraryDataModel.Books?.Select(ToBookDomainModel).ToList()
            };
        }

        private BookDataModel ToBookDataModel(Book book)
        {
            return new BookDataModel
            {
                Id = book.Id,
                Title = book.Value.Title,
                AuthorName = book.Value.AuthorName,
                PublishDate = book.Value.PublishDate
            };
        }

        private Book ToBookDomainModel(BookDataModel bookDataModel)
        {
            var value = new BookValue(bookDataModel.Title, bookDataModel.AuthorName, bookDataModel.PublishDate);
            return new Book(bookDataModel.Id, value);
        }
    }
}