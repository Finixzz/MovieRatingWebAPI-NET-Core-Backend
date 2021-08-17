using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using MovieRatingEngine.DAL.Interfaces;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Utils.DTOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Services.Movies.Commands
{
    public class SaveMovieCommand : IRequest<object>
    {
        public CreateMovieDTO CreateMovieDTO { get; set; }

        public SaveMovieCommand(CreateMovieDTO CreateMovieDTO)
        {
            this.CreateMovieDTO = CreateMovieDTO;
        }
    }

    public class SaveMovieCommandHandler : IRequestHandler<SaveMovieCommand, object>
    {
        private readonly IMovieSQLRepository _movieRepository;

        private readonly ICastSQLRepository _castRepository;

        private readonly IHostingEnvironment _hostingEnvironment;

        public SaveMovieCommandHandler(IMovieSQLRepository _movieRepository, ICastSQLRepository _castRepository,
            IHostingEnvironment _hostingEnvironment)
        {
            this._movieRepository = _movieRepository;
            this._castRepository = _castRepository;
            this._hostingEnvironment = _hostingEnvironment;
        }
        public async Task<object> Handle(SaveMovieCommand request, CancellationToken cancellationToken)
        {
            
            /*
            string uniqueFileName = null;
            if (request.CreateMovieDTO.ImagePath != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "CoverImagesRepo");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + request.CreateMovieDTO.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                request.CreateMovieDTO.ImagePath.CopyTo(new FileStream(filePath, FileMode.Create));

            }
            */
            Movie newMovie = new Movie()
            {
                Title = request.CreateMovieDTO.Title,
                ImagePath = request.CreateMovieDTO.ImagePath,
                Description = request.CreateMovieDTO.Description,
                ReleaseDate = request.CreateMovieDTO.ReleaseDate
            };


            newMovie = await _movieRepository.SaveAsync(newMovie);

            List<Cast> movieCasts = new List<Cast>();
            for(int i = 0; i < request.CreateMovieDTO.Actors.Count; i++)
            {
                Cast movieCast = new Cast()
                {
                    MovieId = newMovie.MovieId,
                    ActorId = request.CreateMovieDTO.Actors.ElementAt(i)
                };
                movieCasts.Add(movieCast);
            }

            await _castRepository.SaveAsync(movieCasts);

            return await _movieRepository.GetByIdAsync(newMovie.MovieId);
        }
    }
}
