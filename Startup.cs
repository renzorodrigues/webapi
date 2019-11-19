using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data.Helpers;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Helpers;
using WebApi.Domain.Repositories;
using WebApi.Domain.Repositories.Interfaces;
using WebApi.Domain.Services;
using WebApi.Domain.Services.Interfaces;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            
            // Helpers DI
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories DI
            services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.AddScoped<IRepositoryBase<Attended>, RepositoryBase<Attended>>();
            services.AddScoped<IRepositoryBase<Contact>, RepositoryBase<Contact>>();
            services.AddScoped<IRepositoryBase<Tutor>, RepositoryBase<Tutor>>();
            services.AddScoped<IAttendedRepository, AttendedRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            // Services DI
            services.AddScoped<IAttendedService, AttendedService>();
            services.AddScoped<IAuthService, AuthService>();

            // CORS configuration
            services.AddCors(options => {
                options.AddPolicy(MyAllowSpecificOrigins, builder => {
                    builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            // JWT Authentication / Configuration
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken =  false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
