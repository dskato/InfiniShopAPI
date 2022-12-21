using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration){

            //Add the services for the JWT
            //The scoped function gonna implement just when token is needed 
            services.AddScoped<ITokenService, TokenService>();
            //Config the connection string
            //We can use SQL too
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer("server=DESKTOP-3GEAH5D\\SQLEXPRESS; database=WikiMovil; Trusted_Connection=True;");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });


            return services;
        }
    }
}