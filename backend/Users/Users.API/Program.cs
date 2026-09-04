using Scalar.AspNetCore;
using Shared.Extensions;
using System.Reflection;
using Users.API.Configuration;
using Users.API.Extensions;
using Users.Application.UnitOfWork;
using Users.Infrastructure.Initialization;
using Users.Infrastructure.UnitOfWork;

namespace Users.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var services = builder.Services;
            var configuration = builder.Configuration;
            var logging = builder.Logging;

            // Add configs
            configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables();
            var sqlConfig = services.ConfigureAndReceive<SqlServerConfig>(configuration, "SqlServerConfig");

            // Infrastructure
            services.AddUserDbContext(sqlConfig);
            services.AddRepositories();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DatabaseInitializer>();

            // Application
            services.AddAutoMapper(_ => { }, Assembly.Load("Users.Application"));
            services.AddValidators();
            services.AddServices();
            services.AddUseCases();

            // API
            services.AddControllers();
            services.AddOpenApi();

            var app = builder.Build();

            // Initializing DB
            using (var scope = app.Services.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<DatabaseInitializer>()
                    .InitializeAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            // app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
