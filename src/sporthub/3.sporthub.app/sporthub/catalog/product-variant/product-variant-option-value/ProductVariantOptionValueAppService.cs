using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;

namespace sporthub.app
{
    public class ProductVariantOptionValueAppService : IProductVariantOptionValueAppService
    {
        private readonly IProductVariantOptionValueRepository _productVariantOptionValueRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductVariantOptionValueAppService(IProductVariantOptionValueRepository productVariantOptionValueRepository, ISportHubUnitOfWork unitOfWork, IMapper mapper)
        {
            _productVariantOptionValueRepository = productVariantOptionValueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
