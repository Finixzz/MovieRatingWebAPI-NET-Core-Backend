using AutoMapper;
using MediatR;
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
    public class GetMovieByIdQuery : IRequest<object>
    {
        public int MovieId { get; set; }

        public GetMovieByIdQuery(int MovieId)
        {
            this.MovieId = MovieId;
        }
    }

    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, object>
    {
        private IMovieSQLRepository _movieRepository;
        private IActorSQLRepository _actorRepository;
        private IMapper _mapper;

        public GetMovieByIdQueryHandler(IMovieSQLRepository _movieRepository, IMapper _mapper, IActorSQLRepository _actorRepository)
        {
            this._movieRepository = _movieRepository;
            this._mapper = _mapper;
            this._actorRepository = _actorRepository;
        }
        public async Task<object> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            Movie movieInDb = await _movieRepository.GetByIdAsync(request.MovieId);
            if (movieInDb == null)
                return null;

            ReadMovieDTO movieDTO = _mapper.Map<Movie, ReadMovieDTO>(movieInDb);
            movieDTO.Actors = _mapper.Map<List<Actor>,List<ReadActorDTO>>(await _actorRepository.GetAllByMovieIdAsync(movieDTO.MovieId));

            if(movieDTO.Ratings.Count!=0)
            {
                movieDTO.TotalRating = (decimal)movieDTO.Ratings.Sum(c => c.Rating1) / movieDTO.Ratings.Count();
            }

            return movieDTO;
        }
    }
}
