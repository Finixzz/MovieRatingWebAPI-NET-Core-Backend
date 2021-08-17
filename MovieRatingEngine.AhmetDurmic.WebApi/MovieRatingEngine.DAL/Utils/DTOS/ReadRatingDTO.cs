using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class ReadRatingDTO
    {
        public int RatingId { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int? Rating1 { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
