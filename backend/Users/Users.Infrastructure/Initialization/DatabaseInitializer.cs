using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Users.Application.Services.Interfaces;
using Users.Domain;
using Users.Infrastructure.Contexts;

namespace Users.Infrastructure.Initialization
{
    public sealed class DatabaseInitializer
    {
        private const string AdminEmail = "admin@gmail.com";
        private const string AdminPassword = "admin";

        private readonly UserDbContext _context;
        private readonly IPasswordHashingService _passwordHashingService;

        public DatabaseInitializer(UserDbContext context, IPasswordHashingService passwordHashingService)
        {
            _context = context;
            _passwordHashingService = passwordHashingService;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.MigrateAsync(cancellationToken);

            var adminExists = await _context.Users.AnyAsync(x => x.Email == EF.Parameter(AdminEmail), cancellationToken);

            if (adminExists)
                return;

            var admin = new User
            {
                Name = "Admin",
                Surname = "Admin",
                Email = AdminEmail,
                PasswordHash = _passwordHashingService.HashPassword(AdminPassword),
                Role = UserRoles.Admin
            };

            _context.Users.Add(admin);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
