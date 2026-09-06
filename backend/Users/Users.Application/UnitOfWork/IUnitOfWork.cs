using Shared.Abstractions.UnitOfWork;
using Users.Application.Repositories;

namespace Users.Application.UnitOfWork
{
    public interface IUnitOfWork : IBaseUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}
