using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces.Implementation
{
    public class CastRepository : ICastSQLRepository
    {
        private MoviesDBContext _appDbContext;

        public CastRepository(MoviesDBContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public async Task<List<Cast>> SaveAsync(List<Cast> casts)
        {
            await _appDbContext.Casts.AddRangeAsync(casts);
            await _appDbContext.SaveChangesAsync();
            return casts;
        }
    }
}
