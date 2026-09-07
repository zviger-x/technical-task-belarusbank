
using Products.API.Extensions;
using Scalar.AspNetCore;
using Shared.Extensions;
using Shared.Middlewares;
using System.Reflection;

namespace Products.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add logging
            builder.Logging.ConfigureLogger(
                microserviceName: Assembly.GetExecutingAssembly().GetName().Name);

            // Add configs
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables();

            // Add application services
            builder.Services.AddCompositionRoot(builder.Configuration);

            var app = builder.Build();

            // Middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Initializing DB
            // await app.InitializeDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options
                        .AddPreferredSecuritySchemes("Bearer")
                        .EnablePersistentAuthentication();
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
