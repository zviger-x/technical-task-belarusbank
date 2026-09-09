using AutoMapper;
using FluentValidation;
using MediatR;
using Products.Application.Clients;
using Products.Application.Common;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Products.Domain;
using Shared.Common.Results;
using Shared.Extensions;

namespace Products.Application.UseCases.Handlers
{
    public class CategoryCreateHandler : BaseHandler, IRequestHandler<CategoryCreateCommand, Result<Guid>>
    {
        private readonly IValidator<CategoryCreateCommand> _validator;
        private readonly IUserClient _userClient;

        public CategoryCreateHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CategoryCreateCommand> validator,
            IUserClient userClient)
            : base(unitOfWork, mapper)
        {
            _validator = validator;
            _userClient = userClient;
        }

        public async Task<Result<Guid>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var isBlocked = await _userClient.IsUserBlockedAsync(request.UserContext.Id, cancellationToken);
            if (isBlocked)
                return Result.Failure<Guid>(UserErrors.UserBlocked);

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<Guid>(validationResult.ToErrors());

            var category = _mapper.Map<Category>(request.Category);

            await _unitOfWork.CategoryRepository.CreateAsync(category, cancellationToken);

            var auditLog = new AuditLog
            {
                Action = AuditActions.CategoryCreated,
                UserId = request.UserContext.Id,
                EntityId = category.Id,
            };
            await _unitOfWork.AuditRepository.CreateAsync(auditLog, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
