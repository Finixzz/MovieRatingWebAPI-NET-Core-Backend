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
    public class GetAllMoviesQuery : IRequest<object>
    {
    }

    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, object>
    {
        private IMovieSQLRepository _movieRepository;
        private IMapper _mapper;

        public GetAllMoviesQueryHandler(IMovieSQLRepository _movieRepository, IMapper _mapper)
        {
            this._movieRepository = _movieRepository;
            this._mapper = _mapper;
        }

        public async Task<object> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            List<ReadMovieDTO> movieDTOs = _mapper.Map<List<Movie>,List<ReadMovieDTO>> (await _movieRepository.GetAllAsync());

            for(int i = 0; i < movieDTOs.Count; i++)
            {
                if(movieDTOs[i].Ratings.Count!=0)
                    movieDTOs[i].TotalRating = (decimal)movieDTOs[i].Ratings.Sum(c => c.Rating1) / movieDTOs[i].Ratings.Count();
                
            }

            return movieDTOs.OrderByDescending(c => c.TotalRating);
        }
    }
}
