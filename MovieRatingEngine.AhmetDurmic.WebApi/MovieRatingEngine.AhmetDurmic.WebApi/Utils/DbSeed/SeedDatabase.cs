using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRatingEngine.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingEngine.AhmetDurmic.WebApi.Utils.DbSeed
{
    public class SeedDatabase
    {
        public static async Task PopulateAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            await SeedDataAsync(serviceScope.ServiceProvider.GetService<MoviesDBContext>());
        }

        private static async Task SeedDataAsync(MoviesDBContext context)
        {
            await context.Database.MigrateAsync();
            if (!context.Roles.Any())
            {
                await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetRoles");
                await context.Roles.AddRangeAsync(
                    new Microsoft.AspNetCore.Identity.IdentityRole()
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new Microsoft.AspNetCore.Identity.IdentityRole()
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    }


                );
                await context.SaveChangesAsync();
            }

            if (!context.Actors.Any())
            {
                await context.Actors.AddRangeAsync(
                  new Actor()
                  { 
                      FirstName = "Jack",
                      LastName = "Nicholson"
                  },
                new Actor()
                {
                    FirstName = "Denzel",
                    LastName = "Washington"
                },
                new Actor()
                {
                    FirstName = "Eddy",
                    LastName = "Murphy"
                },
                    new Actor()
                    {
                        FirstName = "Tom",
                        LastName = "Hanks"
                    },
                    new Actor()
                    {
                        FirstName = "Leonardo",
                        LastName = "DiCaprio"
                    },
                    new Actor()
                    {
                        FirstName = "Morgan",
                        LastName = "Freeman"
                    },
                    new Actor()
                    {
                        FirstName = "Johny",
                        LastName = "Depp"
                    }

               );
                await context.SaveChangesAsync();
            }

        }
    }
}
