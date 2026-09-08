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
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private const int PageSize = 100;

        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AuthorizeRoles(UserRoles.User, UserRoles.SuperUser)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductDto productToCreate, CancellationToken cancellationToken)
        {
            var command = new ProductCreateCommand(productToCreate, User.GetUserContext());

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [AuthorizeRoles(UserRoles.User, UserRoles.SuperUser)]
        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid productId, [FromBody] UpdateProductDto productToUpdate, CancellationToken cancellationToken)
        {
            var command = new ProductUpdateCommand(productId, productToUpdate, User.GetUserContext());

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [AuthorizeRoles(UserRoles.SuperUser)]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            var command = new ProductDeleteCommand(productId, User.GetUserContext());

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToHttpResult();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPagedWithFilterAsync(
            [FromQuery] ProductFilterDto filter,
            [FromQuery] int pageNumber = 1,
            CancellationToken cancellationToken = default)
        {
            var query = new ProductGetPagedWithFilterQuery(
                filter,
                new PageParameters { PageNumber = pageNumber, PageSize = PageSize });

            var result = await _mediator.Send(query, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
