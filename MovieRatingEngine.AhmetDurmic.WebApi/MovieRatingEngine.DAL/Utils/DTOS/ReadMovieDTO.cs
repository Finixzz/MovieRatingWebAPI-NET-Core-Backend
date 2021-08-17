using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class ReadMovieDTO : Movie
    {
        public decimal TotalRating { get; set; }
    }
}
