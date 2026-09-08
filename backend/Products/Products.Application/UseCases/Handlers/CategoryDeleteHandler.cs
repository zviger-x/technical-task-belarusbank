using AutoMapper;
using MediatR;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Shared.Common.Results;

namespace Products.Application.UseCases.Handlers
{
    public class CategoryDeleteHandler : BaseHandler, IRequestHandler<CategoryDeleteCommand, Result>
    {
        public CategoryDeleteHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (entity == null)
                return Result.Failure(CategoryErrors.CategoryToDeleteNotFound);

            // TODO: Add audit log
            await _unitOfWork.CategoryRepository.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
