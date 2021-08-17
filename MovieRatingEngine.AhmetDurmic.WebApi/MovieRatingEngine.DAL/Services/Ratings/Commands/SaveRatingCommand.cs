using MediatR;
using Microsoft.AspNetCore.Http;
using MovieRatingEngine.DAL.Interfaces;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Utils.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TokenDecryptor;

namespace MovieRatingEngine.DAL.Services.Ratings.Commands
{
    public class SaveRatingCommand : IRequest<object>
    {
        public CreateRatingDTO CreateRatingDTO { get; set; }

        public SaveRatingCommand(CreateRatingDTO CreateRatingDTO)
        {
            this.CreateRatingDTO = CreateRatingDTO;
        }
    }

    public class SaveRatingCommandHandler : IRequestHandler<SaveRatingCommand, object>
    {
        private IMediator _broker;
        private IRatingSQLRepository _ratingRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public SaveRatingCommandHandler(IRatingSQLRepository _ratingRepository, IMediator _broker, IHttpContextAccessor _httpContextAccessor)
        {
            this._ratingRepository = _ratingRepository;
            this._broker = _broker;
            this._httpContextAccessor = _httpContextAccessor;
        }
        public async Task<object> Handle(SaveRatingCommand request, CancellationToken cancellationToken)
        {
            Rating newMovieRating = new Rating();
            string userJWT = (string)await new GetUserJWTAsync(_httpContextAccessor).executeAsync();
            if (userJWT == null)
            {
                newMovieRating.UserId = null;
                newMovieRating.MovieId = request.CreateRatingDTO.MovieId;
                newMovieRating.Rating1 = request.CreateRatingDTO.Rating;
                newMovieRating.DateAdded = DateTime.Now;
            }
            else
            {
                string userId = (string)await _broker.Send(new GetUserIdExtractedFromUserAccesTokenAsync());
                newMovieRating.UserId = userId;
                newMovieRating.MovieId = request.CreateRatingDTO.MovieId;
                newMovieRating.Rating1 = request.CreateRatingDTO.Rating;
                newMovieRating.DateAdded = DateTime.Now;

            }
            return await _ratingRepository.SaveAsync(newMovieRating);

        }
    }
}
