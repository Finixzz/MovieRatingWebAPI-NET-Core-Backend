using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Services.Ratings.Commands;
using MovieRatingEngine.DAL.Utils.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingEngine.AhmetDurmic.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private IMediator _broker;

        public RatingsController(IMediator _broker)
        {
            this._broker = _broker;
        }


        [Route("api/[controller]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RateMovieAsync(CreateRatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var createRatingCommand = new SaveRatingCommand(ratingDTO);

            Rating newRating = (Rating) await _broker.Send(createRatingCommand);

            return Created("api/ratings", newRating);
        }

        [Route("api/[controller]/range")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RateMoviesAsync(List<CreateRatingDTO> ratingDTOS)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var createRatingsCommand = new SaveRatingsRangeCommand(ratingDTOS);

            List<Rating> newRatings = (List<Rating>)await _broker.Send(createRatingsCommand);

            return Created("api/ratings", newRatings);
        }


    }
}
