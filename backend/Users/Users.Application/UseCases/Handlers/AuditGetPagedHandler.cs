using AutoMapper;
using FluentValidation;
using MediatR;
using Shared.Common;
using Shared.Common.Results;
using Shared.Extensions;
using Users.Application.UnitOfWork;
using Users.Application.UseCases.Queries;
using Users.Domain;

namespace Users.Application.UseCases.Handlers
{
    public class AuditGetPagedHandler : BaseHandler, IRequestHandler<AuditGetPagedQuery, Result<PagedCollection<AuditLog>>>
    {
        private readonly IValidator<AuditGetPagedQuery> _validator;

        public AuditGetPagedHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<AuditGetPagedQuery> validator)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
        }

        public async Task<Result<PagedCollection<AuditLog>>> Handle(AuditGetPagedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<PagedCollection<AuditLog>>(validationResult.ToErrors());

            var pageNumber = request.PageParameters.PageNumber;
            var pageSize = request.PageParameters.PageSize;

            var paged = await _unitOfWork.AuditRepository.GetPagedByUserAsync(
                request.UserId,
                pageNumber,
                pageSize,
                cancellationToken);

            return Result.Success(paged);
        }
    }
}
