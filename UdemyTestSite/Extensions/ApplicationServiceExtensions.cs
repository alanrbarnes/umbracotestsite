﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using UdemyTestSite.Data;
using UdemyTestSite.Helpers;
using UdemyTestSite.Interfaces;
using UdemyTestSite.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Persistence.Repositories;

namespace UdemyTestSite.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            /*services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            //services.AddCors();
            //added
            services.AddDbContext<DataContext>(options => {
                //options.UseSqlite("connection string");
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });*/
            return services;
        }
    }
}
