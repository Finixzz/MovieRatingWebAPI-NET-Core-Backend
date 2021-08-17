using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Services.Movies.Queries
{
    public class GetTopTenRatedMoviesQuery : IRequest<object>
    {
    }


    public class GetTopTenRatedMoviesQueryHandler : IRequestHandler<GetTopTenRatedMoviesQuery, object>
    {
        private MoviesDBContext _appDbContext;

        public GetTopTenRatedMoviesQueryHandler(MoviesDBContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public async Task<object> Handle(GetTopTenRatedMoviesQuery request, CancellationToken cancellationToken)
        {
            var result = (from r in _appDbContext.Ratings
                          group new { r.Movie, r } by new
                          {
                              r.Movie.MovieId,
                              r.Movie.Title,
                              r.Movie.ImagePath,
                              r.Movie.Description,
                              ReleaseDate = (DateTime?)r.Movie.ReleaseDate
                          } into g
                          select new
                          {
                              g.Key.MovieId,
                              g.Key.Title,
                              g.Key.ImagePath,
                              g.Key.Description,
                              ReleaseDate = (DateTime?)g.Key.ReleaseDate,
                              TotalRating = (decimal)((decimal)g.Sum(p => p.r.Rating1) / g.Count(p => p.r.Rating1 != null))
                          }).Take(10);

            return await result.OrderByDescending(c=>c.TotalRating).ToListAsync();
        }
    }
}
