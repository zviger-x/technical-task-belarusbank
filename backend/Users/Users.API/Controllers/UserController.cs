using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Extensions;
using Users.Application.Contracts;
using Users.Application.UseCases.Commands;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        // [Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserDto userToCreate, CancellationToken cancellationToken)
        {
            var command = new UserCreateCommand(userToCreate);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> ChangeRole([FromRoute] Guid userId, [FromBody] UserRoles userRole, CancellationToken cancellationToken)
        {
            var command = new UserChangeRoleCommand(userId, userRole);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [HttpPatch("{userId}/password")]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid userId, [FromBody] ChangeUserPasswordDto changeUserPasswordDto, CancellationToken cancellationToken)
        {
            var command = new UserChangePasswordCommand(userId, changeUserPasswordDto);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [HttpPatch("{userId}/block")]
        public async Task<IActionResult> ChangeUserBlock([FromRoute] Guid userId, [FromBody] bool isBlocked, CancellationToken cancellationToken)
        {
            var command = new UserChangeBlockCommand(userId, isBlocked);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var command = new UserDeleteCommand(userId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
