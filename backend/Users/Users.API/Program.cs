using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shared.Extensions;
using System.Reflection;
using Users.API.Configuration;
using Users.API.Extensions;
using Users.Application.UnitOfWork;
using Users.Infrastructure.Contexts;
using Users.Infrastructure.UnitOfWork;

namespace Users.API
{
    public class Program
    {
        public static void Main(string[] args)
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
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

                // Auto migrations
                if (dbContext.Database.GetPendingMigrations().Any())
                    dbContext.Database.Migrate();
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
