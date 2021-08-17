using System;
using System.Collections.Generic;

#nullable disable

namespace MovieRatingEngine.DAL.Models
{
    public partial class Actor
    {
        public Actor()
        {
            Casts = new HashSet<Cast>();
        }

        public int ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Cast> Casts { get; set; }
    }
}
