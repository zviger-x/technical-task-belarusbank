namespace Shared.Abstractions.Logging
{
    public interface IAuditLogger
    {
        Task LogAsync(Guid userId, string action, CancellationToken cancellationToken);
    }
}
