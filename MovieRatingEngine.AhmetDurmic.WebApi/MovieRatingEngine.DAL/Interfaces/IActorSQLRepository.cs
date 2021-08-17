using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces
{
    public interface IActorSQLRepository
    {
        Task<List<Actor>> GetAllByMovieIdAsync(int movieId);
    }
}
