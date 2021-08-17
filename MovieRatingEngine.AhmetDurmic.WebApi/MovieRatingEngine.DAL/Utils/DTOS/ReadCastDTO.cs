using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Utils.DTOS
{
    public class ReadCastDTO
    {
        public int CastId { get; set; }
        public int MovieId { get; set; }
        public int ActorId { get; set; }
    }
}
