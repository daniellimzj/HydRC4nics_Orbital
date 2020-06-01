using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Movies.Data;
using EFCoreSample.Movies.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Movies.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDatabaseContext _db;

        public RentalRepository(AppDatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<MovieRental>> GetAll()
        {
            var dataModels = await _db.Rentals
                .Include(rental => rental.Movies)
                .ToListAsync();
            return dataModels.Select(ToDomainModel);
        }

        public async Task<MovieRental> Get(Guid id)
        {
            var dataModel = await _db.Rentals.Include(e => e.Movies)
                .FirstOrDefaultAsync(entity => entity.Id == id);
            return dataModel == null? null : ToDomainModel(dataModel);
        }

        public async Task<MovieRental> Add(MovieRentalValue value)
        {
            if (value == null) return null;
            var rental = new MovieRental(Guid.NewGuid(), value);
            
            await _db.Rentals.AddAsync(ToDataModel(rental));
            await _db.SaveChangesAsync();
            return await Get(rental.Id);
        }

        public async Task<MovieRental> Update(Guid id, MovieRentalValue value)
        {
            var toBeUpdated = await _db.Rentals.Include(e => e.Movies)
                .FirstOrDefaultAsync(entity => entity.Id == id);
            if (toBeUpdated == null) return null;
            
            toBeUpdated.Name = value.Name;
            toBeUpdated.Address = value.Address;

            await _db.SaveChangesAsync();
            return ToDomainModel(toBeUpdated);
        }

        public async Task<bool> Delete(Guid id)
        {
            var toBeDeleted = await _db.Rentals.Include(e => e.Movies)
                .FirstOrDefaultAsync(entity => entity.Id == id);

            if (toBeDeleted == null) return false;
            
            _db.Remove(toBeDeleted);
            await _db.SaveChangesAsync();
            return true;
        }

        private MovieRental ToDomainModel(RentalDataModel dataModel)
        {
            return new MovieRental(dataModel.Id, new MovieRentalValue(dataModel.Name, dataModel.Address))
            {
                Movies = dataModel.Movies?.Select(ToMovieDomainModel).ToList()
            };
        }

        private RentalDataModel ToDataModel(MovieRental rental)
        {
            return new RentalDataModel
            {
                Id = rental.Id,
                Name = rental.Value.Name,
                Address = rental.Value.Address,
                Movies = rental.Movies?.Select(ToMovieDataModel).ToList()
            };
        }
        private Movie ToMovieDomainModel(MovieDataModel dataModel)
        {
            return new Movie(dataModel.Id, 
                new MovieValue(dataModel.Title, dataModel.DirectorName, dataModel.ReleaseDate));
        }
        private MovieDataModel ToMovieDataModel(Movie movie)
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