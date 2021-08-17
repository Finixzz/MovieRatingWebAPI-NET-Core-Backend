using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class CreateRatingDTO
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
    }
}
