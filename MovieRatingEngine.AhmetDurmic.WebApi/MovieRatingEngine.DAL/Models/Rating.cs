using System;
using System.Collections.Generic;

#nullable disable

namespace MovieRatingEngine.DAL.Models
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int? Rating1 { get; set; }
        public DateTime? DateAdded { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
