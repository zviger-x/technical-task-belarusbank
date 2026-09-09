using AutoMapper;
using MediatR;
using Products.Application.Services.Interfaces;
using Products.Application.UnitOfWork;
using Products.Application.UseCases.Queries;
using Shared.Common.Results;

namespace Products.Application.UseCases.Handlers
{
    public class ProductGetCatalogHandler
        : BaseHandler, IRequestHandler<ProductGetCatalogQuery, Result<byte[]>>
    {
        private readonly IPdfCatalogGenerator _pdfCatalogGenerator;

        public ProductGetCatalogHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPdfCatalogGenerator pdfCatalogGenerator)
            : base(unitOfWork, mapper)
        {
            _pdfCatalogGenerator = pdfCatalogGenerator;
        }

        public async Task<Result<byte[]>> Handle(ProductGetCatalogQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.GetForCatalogAsync(
                request.Filter.Name,
                request.Filter.Description,
                request.Filter.GeneralNote,
                request.Filter.SpecialNote,
                request.Filter.CategoryId,
                cancellationToken);

            var pdf = _pdfCatalogGenerator.Generate(products);

            return Result.Success(pdf);
        }
    }
}
