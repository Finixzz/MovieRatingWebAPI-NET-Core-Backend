using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatingEngine.DAL.Interfaces
{
    public interface IActorSQLRepository
    {
        Task<Actor> GetByIdAsync(int id);

        Task<Actor> SaveAsync(Actor actor);

        Task<Actor> EditAsync(Actor actor,int id);

        Task<Actor> DeleteAsync(Actor actor);
    }
}
