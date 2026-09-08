using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.API.Configuration;
using Products.Application.Clients;
using Products.Application.Repositories;
using Products.Domain;
using Products.Infrastructure.Contexts;
using Products.Infrastructure.Grpc.Clients;
using Products.Infrastructure.Repositories;
using Shared.Abstractions.Repositories;
using Shared.Grpc.User;
using System.Reflection;

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
            services.AddValidatorsFromAssembly(Assembly.Load("Products.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("Shared"));
        }

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Products.Application")));
        }

        public static void AddClients(this IServiceCollection services)
        {
            services.AddGrpcClient<UserService.UserServiceClient>(o =>
            {
                o.Address = new Uri("http://users.api:8080");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            services.AddScoped<IUserClient, UserClient>();
        }
    }
}
