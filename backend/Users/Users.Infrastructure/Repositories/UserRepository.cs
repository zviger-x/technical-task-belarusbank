using Microsoft.EntityFrameworkCore;
using Users.Application.Repositories;
using Users.Domain;
using Users.Infrastructure.Contexts;

namespace Users.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(UserDbContext context)
            : base(context)
        {
        }

        public async Task<bool> IsUserBlockedAsync(string email, CancellationToken token = default)
        {
            return await _context.Users
                .Where(x => x.Email == email)
                .Select(x => x.IsBlocked)
                .FirstOrDefaultAsync(token);
        }

        public async Task<bool> ContainsEmailAsync(string email, CancellationToken token = default)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, token);
        }
    }
}
