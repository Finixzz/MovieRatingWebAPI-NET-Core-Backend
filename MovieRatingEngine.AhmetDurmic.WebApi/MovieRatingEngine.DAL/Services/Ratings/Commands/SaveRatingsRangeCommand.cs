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
    public class SaveRatingsRangeCommand : IRequest<object>
    {
        public List<CreateRatingDTO> CreateRatingDTOS { get; set; }

        public SaveRatingsRangeCommand(List<CreateRatingDTO> CreateRatingDTOS)
        {
            this.CreateRatingDTOS = CreateRatingDTOS;
        }
    }

    public class SaveRatingsRangeCommandHandler : IRequestHandler<SaveRatingsRangeCommand,object>
    {
        private IMediator _broker;
        private IRatingSQLRepository _ratingRepository;
        private IHttpContextAccessor _httpContextAccessor;


        public SaveRatingsRangeCommandHandler(IMediator _broker, IRatingSQLRepository _ratingRepository,IHttpContextAccessor _httpContextAccessor)
        {
            this._broker = _broker;
            this._ratingRepository = _ratingRepository;
            this._httpContextAccessor = _httpContextAccessor;
        }


        public async Task<object> Handle(SaveRatingsRangeCommand request, CancellationToken cancellationToken)
        {
            List<Rating> newMovieRatings = new List<Rating>();
            string userJWT = (string)await new GetUserJWTAsync(_httpContextAccessor).executeAsync();
            DateTime currentDateTime = DateTime.Now;
            if (userJWT == null)
            {
                for (int i = 0; i < request.CreateRatingDTOS.Count; i++)
                {
                    Rating newMovieRating = new Rating()
                    {
                        UserId = null,
                        MovieId = request.CreateRatingDTOS[i].MovieId,
                        Rating1 = request.CreateRatingDTOS[i].Rating,
                        DateAdded = currentDateTime
                    };
                    newMovieRatings.Add(newMovieRating);
                }
            }
            else
            {
                string userId = (string)await _broker.Send(new GetUserIdExtractedFromUserAccesTokenAsync());
                for (int i = 0; i < request.CreateRatingDTOS.Count; i++)
                {
                    Rating newMovieRating = new Rating()
                    {
                        UserId = userId,
                        MovieId = request.CreateRatingDTOS[i].MovieId,
                        Rating1 = request.CreateRatingDTOS[i].Rating,
                        DateAdded = currentDateTime
                    };
                    newMovieRatings.Add(newMovieRating);
                }

            }
            return await _ratingRepository.SaveRangeAsync(newMovieRatings);
        }
    }
}
