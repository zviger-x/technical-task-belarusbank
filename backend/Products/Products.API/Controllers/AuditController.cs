using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.UseCases.Queries;
using Shared.Common;
using Shared.Enums;
using Shared.Extensions;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/audit")]
    public class AuditController : ControllerBase
    {
        private const int PageSize = 100;

        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetAuditLogsPaged(
            [FromQuery] Guid? userId = null,
            [FromQuery] int pageNumber = 1,
            CancellationToken cancellationToken = default)
        {
            var pageParameters = new PageParameters { PageNumber = pageNumber, PageSize = PageSize };
            var query = new AuditGetPagedQuery(userId, pageParameters);

            var result = await _mediator.Send(query, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
