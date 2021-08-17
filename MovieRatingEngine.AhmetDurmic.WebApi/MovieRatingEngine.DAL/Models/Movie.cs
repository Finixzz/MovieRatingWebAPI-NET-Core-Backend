using System;
using System.Collections.Generic;

#nullable disable

namespace MovieRatingEngine.DAL.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Casts = new HashSet<Cast>();
            Ratings = new HashSet<Rating>();
        }

        public int MovieId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual ICollection<Cast> Casts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
