using Microsoft.AspNetCore.Identity;
using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;
using System;

namespace sporthub.app
{
    public class BrandAppService : IBrandAppService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BrandAppService(IBrandRepository brandRepository, ISportHubUnitOfWork unitOfWork, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region admin
        public async Task<OperationResult<BrandDTO>> CreateAsync(CreateBrandRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (await _brandRepository.ExistsBySlugAsync(model.slug, null, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            Brand brand = Brand.Create(model.name, model.slug, model.logo_url, model.description);

            await _brandRepository.CreateAsync(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            BrandDTO dto = _mapper.Map<BrandDTO>(brand);
            return OperationResult<BrandDTO>.Success(dto);
        }

        public async Task<OperationResult<BrandDTO>> UpdateAsync(UpdateBrandRequestDTO model, CancellationToken cancellationToken = default)
        {
            Brand brand = await _brandRepository.BrandGetByIdAsync(model.id, cancellationToken);
            if (brand == null) throw new NotFoundException("Không tìm thấy thương hiệu.");

            if (await _brandRepository.ExistsBySlugAsync(model.slug, model.id, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            brand.Update(model.name, model.slug, model.logo_url, model.description);

            _brandRepository.UpdateAsync(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            BrandDTO dto = _mapper.Map<BrandDTO>(brand);
            return OperationResult<BrandDTO>.Success(dto);
        }

        public async Task<OperationResult<bool>> DeleteAsync(DeleteBrandRequestDTO model, CancellationToken cancellationToken = default)
        {
            Brand brand = await _brandRepository.BrandGetByIdAsync(model.id, cancellationToken);
            if (brand == null) throw new NotFoundException("Không tìm thấy thương hiệu.");

            if (brand.products.Any())
                throw new ConflictException("Không thể xóa thương hiệu đang có sản phẩm.");

            _brandRepository.DeleteAsync(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa thương hiệu thành công");
        }
        public async Task<OperationResult<bool>> UpdateActiveAsync(UpdateActiveBrandRequestDTO model, CancellationToken cancellationToken = default)
        {
            Brand brand = await _brandRepository.BrandGetByIdAsync(model.id, cancellationToken);
            if (brand == null) throw new NotFoundException("Không tìm thấy thương hiệu.");

            if (brand.is_active) brand.Deactivate(); else brand.Activate();

            _brandRepository.UpdateAsync(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Cập nhật trạng thái thành công");
        }
        public async Task<OperationResult<BrandDTO>> BrandGetByIdAsync(BrandGetByIdRequestDTO model, CancellationToken cancellationToken = default)
        {
            Brand brand = await _brandRepository.BrandGetByIdAsync(model.id, cancellationToken);
            if (brand == null) throw new NotFoundException("Không tìm thấy thương hiệu.");
            BrandDTO dto = _mapper.Map<BrandDTO>(brand);

            return OperationResult<BrandDTO>.Success(dto);
        }
        public async Task<OperationResult<PagedResult<BrandDTO>>> BrandGetPagedAsync(GetBrandsPagedRequestDTO model, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _brandRepository.BrandGetPagedAsync(
                model.page_number,
                model.page_size,
                model.search_text,
                model.sort_by,
                model.sort_dir,
                cancellationToken);

            var pagedResult = new PagedResult<Brand>(items, total, model.page_number, model.page_size);
            var dtoResult = pagedResult.MapTo(_mapper.Map<BrandDTO>);

            return OperationResult<PagedResult<BrandDTO>>.Success(dtoResult);
        }
        #endregion
    }
}