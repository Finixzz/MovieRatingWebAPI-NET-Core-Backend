using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces.Implementation
{
    public class MovieRepository : IMovieSQLRepository
    {

        private MoviesDBContext _appDbContext;

        public MovieRepository(MoviesDBContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public async Task<Movie> DeleteAsync(int id)
        {
            Movie movieInDb = await GetByIdAsync(id);

            _appDbContext.Remove(movieInDb);

            await _appDbContext.SaveChangesAsync();

            return movieInDb;
        }

        public async Task<Movie> EditAsync(Movie movie, int id)
        {
            Movie movieInDb = await GetByIdAsync(id);
            movieInDb.Title = movie.Title;
            movieInDb.ImagePath = movie.ImagePath;
            movieInDb.Description = movie.Description;
            movieInDb.ReleaseDate = movie.ReleaseDate;

            await _appDbContext.SaveChangesAsync();

            return movie;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _appDbContext.Movies.Include(movies=>movies.Casts).Include(movies=>movies.Ratings).ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _appDbContext.Movies.Where(c => c.MovieId == id).Include(movies => movies.Casts).Include(movies => movies.Ratings).SingleOrDefaultAsync();
        }

        public async Task<Movie> SaveAsync(Movie movie)
        {
            await _appDbContext.Movies.AddAsync(movie);

            await _appDbContext.SaveChangesAsync();

            return movie;
        }
    }
}
