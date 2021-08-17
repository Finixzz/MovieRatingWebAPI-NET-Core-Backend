using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Utils.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces.Implementation
{
    public class ActorRepository : IActorSQLRepository
    {
        private MoviesDBContext _appDbContext;

        public ActorRepository(MoviesDBContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public async Task<List<Actor>> GetAllByMovieIdAsync(int movieId)
        {
            List<int> movieActorIds = await _appDbContext.Casts.Where(c => c.MovieId == movieId).Select(c => c.ActorId).ToListAsync();

            var result = from actors in _appDbContext.Actors
                         where movieActorIds.Contains(actors.ActorId)
                         select new Actor()
                         {
                             ActorId = actors.ActorId,
                             FirstName = actors.FirstName,
                             LastName = actors.LastName
                         };

            return await result.ToListAsync();
        }
    }
}
