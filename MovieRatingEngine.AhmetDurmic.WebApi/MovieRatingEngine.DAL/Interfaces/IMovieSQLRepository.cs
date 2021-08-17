using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces
{
    public interface IMovieSQLRepository
    {
        Task<Movie> GetByIdAsync(int id);

        Task<List<Movie>> GetAllAsync();

        Task<Movie> SaveAsync(Movie movie);

        Task<Movie> EditAsync(Movie movie, int id);

        Task<Movie> DeleteAsync(int id);
    }
}
