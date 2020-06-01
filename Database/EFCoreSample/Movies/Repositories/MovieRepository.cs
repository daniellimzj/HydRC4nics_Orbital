using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Movies.Data;
using EFCoreSample.Movies.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Movies.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDatabaseContext _db;

        public MovieRepository(AppDatabaseContext db)
        {
            _db = db;
        }
        
        public async Task<IEnumerable<Movie>> GetAll()
        {
            return (await _db.Movies.ToListAsync())
                .Select(ToDomainModel);
        }

        public async Task<Movie> Get(Guid id)
        {
            var dataModel = await _db.Movies.FindAsync(id);
            return dataModel == null ? null : ToDomainModel(dataModel);
        }

        public async Task<Movie> Add(Guid rentalId, MovieValue value)
        {
            var toBeAdded = new Movie(Guid.NewGuid(), value);
            var dataModel = ToDataModel(toBeAdded);
            _db.Movies.Add(dataModel);
            
            var rental = await _db.Rentals.FindAsync(rentalId);
            if (rental == null) return null;
            rental.Movies.Add(dataModel);

            await _db.SaveChangesAsync();
            return await Get(toBeAdded.Id);
        }

        public async Task<Movie> Update(Guid movieId, MovieValue value)
        {
            var toBeUpdated = await _db.Movies.FindAsync(movieId);
            if (toBeUpdated == null) return null; 
            
            toBeUpdated.Title = value.Title;
            toBeUpdated.DirectorName = value.DirectorName;
            toBeUpdated.ReleaseDate = value.ReleaseDate;

            await _db.SaveChangesAsync();
            return ToDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Movies.FindAsync(id);
            if (toBeDeleted == null) return false;
            
            _db.Movies.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }

        private Movie ToDomainModel(MovieDataModel dataModel)
        {
            return new Movie(dataModel.Id, 
                new MovieValue(dataModel.Title, dataModel.DirectorName, dataModel.ReleaseDate));
        }

        private MovieDataModel ToDataModel(Movie movie)
        {
            return new MovieDataModel
            {
                Id = movie.Id,
                Title = movie.Value.Title,
                DirectorName = movie.Value.DirectorName,
                ReleaseDate = movie.Value.ReleaseDate
            };
        }
    }
}