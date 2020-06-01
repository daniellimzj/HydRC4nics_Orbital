using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCoreSample.Libraries;
using EFCoreSample.Libraries.Repositories;
using EFCoreSample.Libraries.Services;
using EFCoreSample.Movies.Repositories;
using EFCoreSample.Movies.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace EFCoreSample
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
            var secret = Encoding.ASCII.GetBytes("super_secret_and_kind_of_confidential");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });
            services.AddAuthorization(x =>
            {
                x.AddPolicy("TeacherOnly", policy => policy.RequireClaim("job", "teacher"));
                x.AddPolicy("StudentOnly", policy => policy.RequireClaim("job", "student"));
            });
            services.AddControllers();

            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookRepository, BookRepository>();
            
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            
            services.AddDbContext<AppDatabaseContext>(options =>
                options.UseSqlite("Filename=EFCoreSample.db"));

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "EFCoreSample"}));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            
            app.UseSwagger(); 
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCoreSample"));
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        
    }
}
