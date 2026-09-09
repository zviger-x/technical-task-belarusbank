using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repositories;
using System.Reflection;
using Users.API.Configuration;
using Users.Application.Repositories;
using Users.Application.Services.Interfaces;
using Users.Domain;
using Users.Infrastructure.Contexts;
using Users.Infrastructure.Repositories;
using Users.Infrastructure.Services;

namespace Users.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUserDbContext(this IServiceCollection services, SqlServerConfig sqlConfig)
        {
            services.AddDbContext<UserDbContext>(o => o.UseSqlServer(sqlConfig.ConnectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IRepository<AuditLog>, AuditRepository>();
            services.AddScoped<IAuditRepository, AuditRepository>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.Load("Users.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("Shared"));
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Users.Application")));
        }
    }
}
