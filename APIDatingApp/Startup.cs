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

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();
                // Hacemos la migración inicial de datos
                await Seed.SeedUsers(context);

            } catch (Exception ex)
            {
                var logger = services.GetService<ILogger<Startup>>();
                logger.LogError(ex, "An error occurred during migration");
            }

        }
    }
}