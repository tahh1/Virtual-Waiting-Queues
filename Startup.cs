using FinalProject.Other_classes;
using FinalProject.Other_classes.Security;
using FinalProject.Repositories.Repos;
using FinalProject.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
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
            services.AddSingleton<IMongoClient>(ServiceProvider =>
            {
                
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://taha:mainuserpassword@webservice.6ihnp.mongodb.net/Project?retryWrites=true&w=majority");
                /*Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();*/
                return new MongoClient(settings);


            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    ClockSkew=TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))

                };
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IrefreshTokenGenerator, refreshTokenGenerator>();
            services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();
            services.AddSingleton<IRefreshTokenRepos, RefreshTokenRepos>();
            services.AddSingleton<ITokenCurrentUser, TokenCurrentUser>();
            services.AddMvc();
            services.AddControllers();
            services.AddSingleton<IUserRepos, UserRepos>();
            services.AddSingleton<IBusinessRepos, BusinessRepos>();
            services.AddSingleton<IWaitingRoomRepos, WaitingRoomRepos>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinalProject", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinalProject v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
