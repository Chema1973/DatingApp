using APIDatingApp.Interfaces;
using APIDatingApp.Services;

namespace APIDatingApp.Extensions
{
    public static class PruebasServiceExtensions
    {
     public static IServiceCollection AddPruebasServiceExtensions(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddScoped<IPruebaService, PruebaService>();
            // services.AddScoped<PruebaService>();
            // De momento "Scoped"

            // services.AddTransient<IPruebaServiceTransient, TransientPruebaService>();
            // services.AddScoped<IPruebaServiceScoped, ScopedPruebaService>();
            // services.AddSingleton<IPruebaServiceSingleton, SingletonPruebaService>();

            services.AddTransient<TransientPruebaService>();
            services.AddScoped<ScopedPruebaService>();
            services.AddSingleton<SingletonPruebaService>();
            

            // Petición A
            //  Transient distintos
            //  Scoped iguales
            //  Singleton iguales
            // Petición B
            //  Transient distintos (y distintos que Petición A)
            //  Scoped iguales (y distintos que Petición A)
            //  Singleton iguales (e iguales que Petición A)

            // --> El valor de transient siempre será dintinto aún en las mismas peticiones
            // --> El valor de scoped siempre será el mismo en una petición y distinto por peticiones
            // --> El valor de Singleton será siempre el mismo en todas las peticiones

            return services;
        }   
    }
}