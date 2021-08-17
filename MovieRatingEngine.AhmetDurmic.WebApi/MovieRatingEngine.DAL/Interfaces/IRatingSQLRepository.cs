using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces
{
    public interface IRatingSQLRepository
    {
        Task<Rating> SaveAsync(Rating rating);

        Task<List<Rating>> SaveRangeAsync(List<Rating> ratings);
    }
}
