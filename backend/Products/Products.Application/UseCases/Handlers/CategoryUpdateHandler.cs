using AutoMapper;
using MediatR;
using Products.Application.Common.Errors;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Commands;
using Shared.Common.Results;

namespace Products.Application.UseCases.Handlers
{
    public class CategoryUpdateHandler : BaseHandler, IRequestHandler<CategoryUpdateCommand, Result>
    {
        public CategoryUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<Result> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Category.Id, cancellationToken);
            if (entity == null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            _mapper.Map(request.Category, entity);

            // TODO: Add audit log
            await _unitOfWork.CategoryRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
