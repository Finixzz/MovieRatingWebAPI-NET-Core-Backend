using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieRatingEngine.AhmetDurmic.WebApi.Services.Commands;
using MovieRatingEngine.AhmetDurmic.WebApi.Utils.DbSeed;
using MovieRatingEngine.DAL.Interfaces;
using MovieRatingEngine.DAL.Interfaces.Implementation;
using MovieRatingEngine.DAL.Models;
using MovieRatingEngine.DAL.Services.Movies.Commands;
using MovieRatingEngine.DAL.Services.Ratings.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenDecryptor;

namespace MovieRatingEngine.AhmetDurmic.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<MoviesDBContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("conn"))
               .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<MoviesDBContext>();

            services.AddAutoMapper(typeof(DAL.Profiles.MappingProfile));


            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);

                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieRatingEngine.AhmetDurmic.WebApi", Version = "v1" });
            });

            services.AddCors(c =>
            {
                c.AddPolicy("MyPolicy", options => options.AllowAnyOrigin());
            });


            services.AddScoped<IMovieSQLRepository, MovieRepository>();

            services.AddScoped<ICastSQLRepository, CastRepository>();

            services.AddScoped<IRatingSQLRepository, RatingRepository>();

            services.AddMediatR(typeof(CreateUserCommand).Assembly);

            services.AddMediatR(typeof(SaveMovieCommand).Assembly);

            services.AddMediatR(typeof(SaveRatingCommand).Assembly);

            services.AddMediatR(typeof(GetUserIdExtractedFromUserAccesTokenAsync).Assembly);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieRatingEngine.AhmetDurmic.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthentication();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedDatabase.PopulateAsync(app).Wait();
        }
    }
}
