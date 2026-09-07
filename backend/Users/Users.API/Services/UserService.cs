using Grpc.Core;
using MediatR;
using Shared.Grpc.User;
using Users.Application.UseCases.Queries;
using GrpcUserService = Shared.Grpc.User.UserService;

namespace Users.API.Services
{
    public class UserService : GrpcUserService.UserServiceBase
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<UserBlockStatusResponse> GetUserBlockStatus(UserBlockStatusRequest request, ServerCallContext context)
        {
            var query = new UserGetBlockStatusQuery(Guid.Parse(request.UserId));

            var response = await _mediator.Send(query, context.CancellationToken);

            if (!response.IsSuccess)
                throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."));

            return new UserBlockStatusResponse { IsBlocked = response.Data };
        }
    }
}
