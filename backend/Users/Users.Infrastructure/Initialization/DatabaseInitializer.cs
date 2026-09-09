using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Users.Application.Services.Interfaces;
using Users.Domain;
using Users.Infrastructure.Contexts;

namespace Users.Infrastructure.Initialization
{
    public sealed class DatabaseInitializer
    {
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

            await SeedDemoData(cancellationToken);
        }

        private async Task SeedDemoData(CancellationToken cancellationToken)
        {
            await TryAddUser(UserRoles.Admin, "admin@gmail.com", "admin", "admin", cancellationToken);
            await TryAddUser(UserRoles.SuperUser, "super@gmail.com", "super", "super", cancellationToken);
            await TryAddUser(UserRoles.User, "user@gmail.com", "user", "user", cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            async Task TryAddUser(UserRoles role, string email, string password, string name, CancellationToken token)
            {
                if (await _context.Users.AnyAsync(x => x.Email == EF.Parameter(email), token))
                    return;

                var user = new User
                {
                    Role = role,
                    Email = email,
                    PasswordHash = _passwordHashingService.HashPassword(password),
                    Name = name,
                    Surname = name,
                };

                _context.Users.Add(user);
            }
        }
    }
}
