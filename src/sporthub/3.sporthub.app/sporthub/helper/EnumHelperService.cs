using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;

namespace sporthub.app
{
    public class EnumHelperService : IEnumHelperService
    {
        public async Task<OperationResult<List<EnumItemDTO>>> GetEnum(EnumRequestDTO model, CancellationToken cancellationToken = default)
        {
            List<EnumItemDTO> data = model.type switch
            {
                "product_status" => EnumHelper.ToList<ProductStatus>(),
                // "discount-type" => EnumHelper.ToList<DiscountType>(),
                _ => throw new NotFoundException($"Không tìm thấy enum: {model.type}")
            };

            return OperationResult<List<EnumItemDTO>>.Success(data);
        }
    }
}