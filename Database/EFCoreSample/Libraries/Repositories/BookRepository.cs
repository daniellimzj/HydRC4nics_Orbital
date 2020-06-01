using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Data;
using EFCoreSample.Libraries.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Libraries.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDatabaseContext _db;

        public BookRepository(AppDatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return (await _db.Books.ToListAsync())
                .Select(ToBookDomainModel);
        }

        public async Task<Book> Get(Guid id)
        {
            var dataModel = await _db.Books.FindAsync(id);
            return dataModel == null ? null : ToBookDomainModel(dataModel);
        }

        public async Task<Book> Add(Guid libraryId, BookValue value)
        {
            var toBeAdded = new Book(Guid.NewGuid(), value);
            var dataModel = ToBookDataModel(toBeAdded);
            _db.Books.Add(dataModel);
            
            var library = await _db.Libraries.FindAsync(libraryId);
            if (library == null) return null;
            library.Books.Add(dataModel);

            await _db.SaveChangesAsync();
            return await Get(toBeAdded.Id);
        }

        public async Task<Book> Update(Guid bookId, BookValue value)
        {
            var toBeUpdated = await _db.Books.FindAsync(bookId);
            if (toBeUpdated == null) return null; 
            
            toBeUpdated.Title = value.Title;
            toBeUpdated.AuthorName = value.AuthorName;
            toBeUpdated.PublishDate = value.PublishDate;

            await _db.SaveChangesAsync();
            return ToBookDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Books.FindAsync(id);
            if (toBeDeleted == null) return false;
            
            _db.Books.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
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