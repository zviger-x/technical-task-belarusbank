using Shared.Configuration;
using Shared.Extensions;
using System.Reflection;
using Users.API.Configuration;
using Users.Application.UnitOfWork;
using Users.Infrastructure.Initialization;
using Users.Infrastructure.UnitOfWork;

namespace Users.API.Extensions
{
    public static class DICompositionRootExtensions
    {
        public static IServiceCollection AddCompositionRoot(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConfig = services.ConfigureAndReceive<SqlServerConfig>(configuration, "SqlServerConfig");
            var jwtConfig = services.ConfigureAndReceive<JwtTokenConfig>(configuration, "Jwt");

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

            // JWT
            services.AddJwtAuthentication(jwtConfig);
            services.AddAuthorization();

            // API
            services.AddGrpc();
            services.AddControllers();
            services.AddScalar();

            return services;
        }
    }
}
