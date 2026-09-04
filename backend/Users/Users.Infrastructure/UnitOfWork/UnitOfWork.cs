using Microsoft.Extensions.DependencyInjection;
using Users.Application.Repositories;
using Users.Application.UnitOfWork;
using Users.Infrastructure.Contexts;

namespace Users.Infrastructure.UnitOfWork
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public IUserRepository UserRepository => _userRepository.Value;
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository.Value;

        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;

        public UnitOfWork(UserDbContext context, IServiceProvider serviceProvider)
            : base(context, serviceProvider)
        {
            _userRepository = new Lazy<IUserRepository>(_serviceProvider.GetRequiredService<IUserRepository>);
            _refreshTokenRepository = new Lazy<IRefreshTokenRepository>(_serviceProvider.GetRequiredService<IRefreshTokenRepository>);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_userRepository.IsValueCreated)
                _userRepository.Value.Dispose();

            if (_refreshTokenRepository.IsValueCreated)
                _refreshTokenRepository.Value.Dispose();
        }
    }
}
