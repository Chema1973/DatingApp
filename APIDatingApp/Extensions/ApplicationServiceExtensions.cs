using APIDatingApp.Data;
using APIDatingApp.Interfaces;
using APIDatingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace APIDatingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt => {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}