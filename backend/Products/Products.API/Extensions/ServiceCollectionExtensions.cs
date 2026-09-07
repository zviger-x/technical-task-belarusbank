using Microsoft.EntityFrameworkCore;
using Products.API.Configuration;
using Products.Application.Repositories;
using Products.Domain;
using Products.Infrastructure.Contexts;
using Products.Infrastructure.Repositories;
using Shared.Abstractions.Repositories;

namespace Products.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUserDbContext(this IServiceCollection services, SqlServerConfig sqlConfig)
        {
            services.AddDbContext<ProductsDbContext>(o => o.UseSqlServer(sqlConfig.ConnectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            // services.AddValidatorsFromAssembly(Assembly.Load("Users.Application"));
            // services.AddValidatorsFromAssembly(Assembly.Load("Shared"));
        }

        public static void AddServices(this IServiceCollection services)
        {
            // services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            // services.AddScoped<ITokenService, TokenService>();
        }

        public static void AddUseCases(this IServiceCollection services)
        {
            // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Users.Application")));
        }
    }
}
