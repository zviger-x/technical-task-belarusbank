using Shared.Entities;

namespace Users.Domain
{
    public class AuditLog : IEntity
    {
        public Guid Id { get; set; }

        public required Guid UserId { get; set; }

        public required string Action { get; set; }

        public Guid? EntityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
