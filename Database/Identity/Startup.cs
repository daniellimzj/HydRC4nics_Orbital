using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Account;
using Identity.Account.Domain;
using Identity.Account.Payload;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Identity
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
            services.AddControllers();

            //Add db context
            services.AddDbContext<AppDatabaseContext>(options =>
                options.UseSqlite("Filename=Identity.db"));

            services.AddIdentity<AppUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDatabaseContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });


            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 123abc'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            // Registering Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIdentityService, IdentityService>();
            
            var secret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));

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
                x.AddPolicy("NUSOnly", policy => policy.RequireClaim("org", "nus"));
                x.AddPolicy("MasterOnly", policy => policy.RequireClaim("job", "master"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory)
        {
            if (env.IsDevelopment())
            {
                // Allow cors
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            await AddMasterUser(serviceScopeFactory);
        }

        private static async Task AddMasterUser(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                var (users, success) = await accountService.GetUsersByClaim(
                    new ClaimRequestModel {Type = "job", Value = "master"});
                
                if (!success)
                {
                    Console.WriteLine("Could not get users");
                    return;
                }

                if (users.Any())
                {
                    Console.WriteLine("Master user already exists");
                    return;
                }

                var masterDetails = Environment.GetEnvironmentVariable("MASTER_USER")?.Split("/");
                if (masterDetails == null)
                {
                    Console.WriteLine("Could not open master details");
                    return;
                }

                Console.WriteLine(masterDetails[0]);
                Console.WriteLine(masterDetails[1]);
                Console.WriteLine(masterDetails[2]);

                var (_, regSuccess) = await accountService.Register(new RegisterRequestModel
                {
                    Email = masterDetails[0],
                    Name = masterDetails[1],
                    Password = masterDetails[2]
                });
                if (!regSuccess)
                {
                    Console.WriteLine("Could not register master");
                    return;
                }

                var (_, claimSuccess) = await accountService.AddClaim(new ClaimRequestModel
                    {Email = masterDetails[0], Type = "job", Value = "master"});
                if (claimSuccess) return;
                Console.WriteLine("Could not add master claim");
            }
        }
    }
}