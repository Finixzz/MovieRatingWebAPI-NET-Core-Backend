using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces.Implementation
{
    public class RatingRepository : IRatingSQLRepository
    {
        private MoviesDBContext _appDbContext;


        public RatingRepository(MoviesDBContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public async Task<Rating> SaveAsync(Rating rating)
        {
            _appDbContext.Ratings.Add(rating);
            await _appDbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<List<Rating>> SaveRangeAsync(List<Rating> ratings)
        {
            _appDbContext.Ratings.AddRange(ratings);
            await _appDbContext.SaveChangesAsync();
            return ratings;
        }
    }
}
