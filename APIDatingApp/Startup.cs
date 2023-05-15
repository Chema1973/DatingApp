using APIDatingApp.Data;
using APIDatingApp.Services;
using APIDatingApp.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APIDatingApp.Extensions;
using APIDatingApp.Middleware;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using APIDatingApp.Entities;
using APIDatingApp.SignalR;

namespace APIDatingApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // services.AddControllers().AddJsonOptions(x =>
            // x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            
            // .AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddApplicationServices(Configuration);

            services.AddIdentityServices(Configuration);

            services.AddPruebasServiceExtensions(Configuration);

        }

        /// Middleware
        public async void  Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Añadimos nuestro propio control de errores
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                // app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials() // --> Por SignalR
                .WithOrigins("https://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                // --> SignalR. Para identificar que usuario está conectado
                endpoints.MapHub<MessageHub>("hubs/message");
                // --> SignalR. Para identificar que usuario está enviando mensajes
            });



            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                await context.Database.MigrateAsync();
                // context.Connections.RemoveRange(context.Connections);

                // await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
                // --> Esto es para SQL
                await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
                // --> Esto es para SQLLite

                // Hacemos la migración inicial de datos
                await Seed.SeedUsers(userManager, roleManager);

            } catch (Exception ex)
            {
                var logger = services.GetService<ILogger<Startup>>();
                logger.LogError(ex, "An error occurred during migration");
            }

        }
    }
}