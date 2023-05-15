using APIDatingApp.Data;
using APIDatingApp.Helpers;
using APIDatingApp.Interfaces;
using APIDatingApp.Services;
using APIDatingApp.SignalR;
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

            services.AddSignalR();
            // --> Añadimos SignalR
            services.AddSingleton<PresenceTracker>();
            // --> Uno de los casos en lo que tenemos que usar Singleton
            //     Queremos que el servicio esté presente desde que se arranca la aplicación
            //     y no nos vale que sea por solicutud Http (scoped)



            return services;
        }
    }
}