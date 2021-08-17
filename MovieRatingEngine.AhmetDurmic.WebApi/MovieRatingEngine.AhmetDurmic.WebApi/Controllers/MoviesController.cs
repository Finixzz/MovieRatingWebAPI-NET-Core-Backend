using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.AhmetDurmic.WebApi.Utils;
using MovieRatingEngine.DAL.Interfaces;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Services.Movies.Commands;
using MovieRatingEngine.DAL.Services.Movies.Queries;
using MovieRatingEngine.DAL.Utils.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingEngine.AhmetDurmic.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _broker;
        private readonly IMovieSQLRepository _movieRepository;

        public MoviesController(IMediator _broker, IMovieSQLRepository _movieRepository)
        {
            this._broker = _broker;
            this._movieRepository = _movieRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/[controller]")]
        public async Task<IActionResult> SaveMovieAsync(CreateMovieDTO movieDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (movieDTO.Actors.Count() < 2)
                return BadRequest(ResponseConstants.CAST_CONSTRAINT_BAD_REQUEST);

            var saveMovieCommand = new SaveMovieCommand(movieDTO);


            Movie newMovie = (Movie)await _broker.Send(saveMovieCommand);

            return Created("api/movies",newMovie);
        }

        [HttpGet]
        [Route("api/[controller]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMoviesAsync()
        {
            var getAllMoviesQuery = new GetAllMoviesQuery();
            return Ok(await _broker.Send(getAllMoviesQuery));
        }


        [HttpGet]
        [Route("api/[controller]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            var getMovieByIdQuery = new GetMovieByIdQuery(id);

            ReadMovieDTO movieDTO = (ReadMovieDTO)await _broker.Send(getMovieByIdQuery);

            if (movieDTO == null)
                return NotFound();

            return Ok(movieDTO);
        }

        [HttpGet]
        [Route("api/[controller]/toptenrated")]
        [AllowAnonymous]

        public async Task<IActionResult> GetTopTenRatedMoviesAsync()
        {
            var getTopTenRatedMoviesQuery = new GetTopTenRatedMoviesQuery();

            return Ok(await _broker.Send(getTopTenRatedMoviesQuery));
        }
    }
}
