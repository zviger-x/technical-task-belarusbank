using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            var command = new UserCreateCommand { User = userToCreate };

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
