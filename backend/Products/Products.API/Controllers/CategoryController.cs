using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Contracts;
using Products.Application.UseCases.Commands;
using Products.Application.UseCases.Queries;
using Shared.Attributes;
using Shared.Common;
using Shared.Enums;
using Shared.Extensions;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private const int PageSize = 100;

        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AuthorizeRoles(UserRoles.SuperUser)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDto categoryToCreate, CancellationToken cancellationToken)
        {
            var command = new CategoryCreateCommand(categoryToCreate, User.GetUserContext());

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [AuthorizeRoles(UserRoles.SuperUser)]
        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid categoryId, [FromBody] UpdateCategoryDto categoryToUpdate, CancellationToken cancellationToken)
        {
            var command = new CategoryUpdateCommand(categoryId, categoryToUpdate, User.GetUserContext());

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [AuthorizeRoles(UserRoles.SuperUser)]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid categoryId, CancellationToken cancellationToken)
        {
            var command = new CategoryDeleteCommand(categoryId, User.GetUserContext());

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPagedWithFilterAsync(
            [FromQuery] int pageNumber = 1,
            CancellationToken cancellationToken = default)
        {
            var query = new CategoryGetPagedQuery(
                new PageParameters { PageNumber = pageNumber, PageSize = PageSize });

            var result = await _mediator.Send(query, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
