using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using EFCoreSample.Controls.Domain;
using EFCoreSample.Controls.Repositories;
using EFCoreSample.Controls.Services;
using EFCoreSample.Monitoring.Domain;
using EFCoreSample.Monitoring.Repositories;
using EFCoreSample.Monitoring.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                x.AddPolicy("OperatorOnly", policy => policy.RequireClaim("job", "operator", "master"));
                x.AddPolicy("AnalystOnly", policy => policy.RequireClaim("job", "analyst", "master"));
            });
            services.AddControllers();

            services.AddSingleton<IPort, Port>();
            services.AddScoped<ISerialRead, SerialRead>();
            services.AddScoped<ISerialWrite, SerialWrite>();
            services.AddSingleton<ISerialCancellation, SerialCancellation>();

            services.AddScoped<IMonitoringConverter, MonitoringConverter>();
            services.AddScoped<IControlsConverter, ControlsConverter>();
            
            services.AddScoped<ISensorService, SensorService>();
            services.AddScoped<ISensorRepository, SensorRepository>();
            services.AddScoped<IReadingService, ReadingService>();
            services.AddScoped<IReadingRepository, ReadingRepository>();
            
            services.AddScoped<IActuatorService, ActuatorService>();
            services.AddScoped<IActuatorRepository, ActuatorRepository>();
            services.AddScoped<ICommandService, CommandService>();
            services.AddScoped<ICommandRepository, CommandRepository>();
            
            services.AddDbContext<AppDatabaseContext>(options =>
                options.UseSqlite("Filename=EFCoreSample.db"));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "EFCoreSample", Version = "v1"});
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
