using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Shared.Configuration;
using System.Text;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Binds a configuration section to a strongly-typed configuration class, 
        /// registers it with the service collection, and returns the configured instance.
        /// </summary>
        /// <typeparam name="TConfig">The type of the configuration class to bind to.</typeparam>
        /// <param name="services">The service collection to register the configuration with.</param>
        /// <param name="configuration">The application's configuration root.</param>
        /// <param name="sectionName">The name of the configuration section to bind.</param>
        /// <returns>The registered configuration instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified configuration section is not found or the configuration object could not be created.
        /// </exception>
        public static TConfig ConfigureAndReceive<TConfig>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where TConfig : class, new()
        {
            var section = configuration.GetSection(sectionName);

            if (!section.Exists())
                throw new InvalidOperationException($"Configuration section '{sectionName}' not found.");

            var config = section.Get<TConfig>();
            if (config == null)
                throw new InvalidOperationException($"Failed to create configuration object for section '{sectionName}'.");

            services.Configure<TConfig>(section);
            return config;
        }

        /// <summary>
        /// Adds JWT Bearer authentication using the specified token configuration.
        /// </summary>
        /// <param name="services">The service collection to register authentication services with.</param>
        /// <param name="config">The JWT token configuration containing issuer, audience, and secret key.</param>
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, JwtTokenConfig config)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        /// <summary>
        /// Configures OpenAPI documentation with JWT Bearer authentication support for Scalar.
        /// </summary>
        public static void AddScalar(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Components ??= new OpenApiComponents();

                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["Bearer"] =
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT"
                        };

                    return Task.CompletedTask;
                });

                options.AddOperationTransformer((operation, context, _) =>
                {
                    var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                    if (metadata.OfType<AllowAnonymousAttribute>().Any())
                        return Task.CompletedTask;

                    if (metadata.OfType<AuthorizeAttribute>().Any())
                    {
                        operation.Security =
                        [
                            new OpenApiSecurityRequirement
                            {
                                [new OpenApiSecuritySchemeReference("Bearer")] = []
                            }
                        ];
                    }

                    return Task.CompletedTask;
                });
            });
        }
    }
}
