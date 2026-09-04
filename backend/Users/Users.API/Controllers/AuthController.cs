using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using System.Security.Claims;
using Users.Application.Contracts;
using Users.Application.UseCases.Commands;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(loginDto);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            // TODO: fix 
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var command = new LogoutCommand(userId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(refreshToken);

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
