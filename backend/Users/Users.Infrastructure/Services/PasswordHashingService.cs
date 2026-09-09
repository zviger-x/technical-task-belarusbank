using Microsoft.AspNetCore.Identity;
using Users.Application.Services.Interfaces;

namespace Users.Infrastructure.Services
{
    public class PasswordHashingService : IPasswordHashingService
    {
        private readonly IPasswordHasher<string> _passwordHasher;

        public PasswordHashingService()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(password, password);
        }

        public bool VerifyPassword(string password, string storedHashedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(password, storedHashedPassword, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
