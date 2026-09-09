using Products.Application.Clients;
using Shared.Grpc.User;

namespace Products.Infrastructure.Grpc.Clients
{
    public class UserClient : IUserClient
    {
        private readonly UserService.UserServiceClient _userServiceClient;

        public UserClient(UserService.UserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<bool> IsUserBlockedAsync(Guid id, CancellationToken cancellationToken)
        {
            var request = new UserBlockStatusRequest { UserId = id.ToString() };

            var result = await _userServiceClient.GetUserBlockStatusAsync(request, cancellationToken: cancellationToken);

            return result.IsBlocked;
        }
    }
}
