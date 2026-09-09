using Shared.Enums;

namespace Users.Application.Contracts
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
