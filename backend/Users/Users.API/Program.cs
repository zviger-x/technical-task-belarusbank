using Microsoft.AspNetCore.Server.Kestrel.Core;
using Scalar.AspNetCore;
using Shared.Extensions;
using Shared.Middlewares;
using System.Reflection;
using Users.API.Extensions;
using Users.API.Services;

namespace Users.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 8080 = rest, 8081 = grpc
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(8080, o => { o.Protocols = HttpProtocols.Http1; });
                options.ListenAnyIP(8081, o => { o.Protocols = HttpProtocols.Http2; });
            });

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
            await app.InitializeDatabaseAsync();

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
            app.MapGrpcService<UserService>();

            app.Run();
        }
    }
}
