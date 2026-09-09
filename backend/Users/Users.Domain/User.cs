using Shared.Entities;
using Shared.Enums;

namespace Users.Domain
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRoles Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
