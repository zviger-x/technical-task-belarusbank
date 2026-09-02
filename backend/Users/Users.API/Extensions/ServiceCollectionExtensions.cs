using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using Users.API.Configuration;
using Users.Application.Repositories;
using Users.Domain;
using Users.Infrastructure.Contexts;
using Users.Infrastructure.Repositories;

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
        }
    }
}
