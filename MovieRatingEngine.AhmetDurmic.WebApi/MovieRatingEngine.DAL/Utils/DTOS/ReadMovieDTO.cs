using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class ReadMovieDTO
    {
        public ReadMovieDTO()
        {
            Ratings = new HashSet<ReadRatingDTO>();
            Actors = new HashSet<ReadActorDTO>();
    }

    public int MovieId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual ICollection<ReadActorDTO> Actors { get; set; }

        public virtual ICollection<ReadRatingDTO> Ratings { get; set; }


        public decimal TotalRating { get; set; }
    }
}
