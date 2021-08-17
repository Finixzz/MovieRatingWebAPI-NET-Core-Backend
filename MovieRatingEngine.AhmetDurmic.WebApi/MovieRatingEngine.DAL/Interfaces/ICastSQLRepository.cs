using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces
{
    public interface ICastSQLRepository
    {
        Task<List<Cast>> SaveAsync(List<Cast> casts);

    }
}
