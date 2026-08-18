using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IBrandAppService
    {
        #region admin
        Task<OperationResult<BrandDTO>> CreateAsync(CreateBrandRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<BrandDTO>> UpdateAsync(UpdateBrandRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteAsync(DeleteBrandRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> UpdateActiveAsync(UpdateActiveBrandRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<BrandDTO>> BrandGetByIdAsync(BrandGetByIdRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<PagedResult<BrandDTO>>> BrandGetPagedAsync(GetBrandsPagedRequestDTO model, CancellationToken cancellationToken = default);
        #endregion
    }
}