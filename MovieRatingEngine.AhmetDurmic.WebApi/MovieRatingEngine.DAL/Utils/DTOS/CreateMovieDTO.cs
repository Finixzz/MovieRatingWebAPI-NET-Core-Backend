using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class CreateMovieDTO
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        ///public IFormFile ImagePath { get; set; }
        ///
        [StringLength(400)]
        public string ImagePath { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public List<int> Actors { get; set; }


        public CreateMovieDTO()
        {
            this.Actors = new List<int>();
        }
    }
}
