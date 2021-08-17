using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRatingEngine.DAL.Interfaces;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Utils.DTOS;
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
        private IActorSQLRepository _actorRepository;
        private IMapper _mapper;

        public GetTopTenRatedMoviesQueryHandler(MoviesDBContext _appDbContext, IMapper _mapper, IActorSQLRepository _actorRepository)
        {
            this._appDbContext = _appDbContext;
            this._actorRepository = _actorRepository;
            this._mapper = _mapper;
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
                          select new ReadMovieDTO
                          {
                              MovieId= g.Key.MovieId,
                              Title=g.Key.Title,
                              ImagePath=g.Key.ImagePath,
                              Description=g.Key.Description,
                              ReleaseDate = (DateTime?)g.Key.ReleaseDate,
                              TotalRating = (decimal)((decimal)g.Sum(p => p.r.Rating1) / g.Count(p => p.r.Rating1 != null))
                          });


            var tempResult = result.OrderByDescending(c => c.TotalRating).Take(10).ToListAsync();

            List<ReadMovieDTO> movieDTOs = tempResult.Result;

            int movieId = movieDTOs[0].MovieId;

            for(int i = 0; i < movieDTOs.Count; i++)
            {
                movieDTOs[i].Actors = _mapper.Map<List<Actor>,List<ReadActorDTO>>( await _actorRepository.GetAllByMovieIdAsync(movieId));
                movieDTOs[i].Ratings = _mapper.Map<List<Rating>,List<ReadRatingDTO>>(await _appDbContext.Ratings.Where(c => c.MovieId == movieId).ToListAsync());
            }

            return movieDTOs;
        }
    }
}
