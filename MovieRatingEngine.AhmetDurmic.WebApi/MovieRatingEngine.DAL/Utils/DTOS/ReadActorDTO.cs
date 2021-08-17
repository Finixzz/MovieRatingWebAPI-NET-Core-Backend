using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class ReadActorDTO
    {
        public int ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
