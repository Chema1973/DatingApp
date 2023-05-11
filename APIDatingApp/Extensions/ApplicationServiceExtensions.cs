using APIDatingApp.Data;
using APIDatingApp.Helpers;
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
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            // --> Cogemos la configuración de Cloudinary
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddScoped<LogUserActivity>();
            // --> Añadimos el filtro de actividad del usuario

            services.AddScoped<ILikesRepository, LikesRepository>();
            
            services.AddScoped<IMessageRepository, MessageRepository>();

            return services;
        }
    }
}