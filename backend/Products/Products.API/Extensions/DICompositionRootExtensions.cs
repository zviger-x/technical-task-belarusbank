using Products.API.Configuration;
using Products.Application.UnitOfWork;
using Products.Infrastructure.UnitOfWork;
using Shared.Configuration;
using Shared.Extensions;
using System.Reflection;

namespace Products.API.Extensions
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
            // services.AddScoped<DatabaseInitializer>();

            // Application
            services.AddAutoMapper(_ => { }, Assembly.Load("Products.Application"));
            services.AddValidators();
            services.AddUseCases();

            // JWT
            services.AddJwtAuthentication(jwtConfig);
            services.AddAuthorization();

            // API
            services.AddControllers();
            services.AddScalar();

            return services;
        }
    }
}
