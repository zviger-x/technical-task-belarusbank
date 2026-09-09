using AutoMapper;
using Products.Application.UnitOfWork;

namespace Products.Application.UseCases.Handlers
{
    public abstract class BaseHandler : IDisposable
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual void Dispose() => _unitOfWork.Dispose();
    }
}
