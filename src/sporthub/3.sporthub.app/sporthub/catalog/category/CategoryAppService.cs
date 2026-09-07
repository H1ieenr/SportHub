using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;

namespace sporthub.app
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        public CategoryAppService(ICategoryRepository categoryRepository, ISportHubUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }
        #region admin
        public async Task<OperationResult<CategoryDTO>> CreateAsync(CreateCategoryRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (await _categoryRepository.ExistsBySlugAsync(model.slug, null, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            Category category = Category.Create(model.name, model.slug, model.parent_id, model.description, model.display_order, model.is_active);

            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultUpload = await _cloudinaryService.UploadImageAsync(model.file_image, $"Catalog/Category/{category.id}");
            if (resultUpload.IsSuccess)
            {
                category.UpdateLogo(resultUpload.Url, resultUpload.PublicId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            CategoryDTO dto = _mapper.Map<CategoryDTO>(category);
            return OperationResult<CategoryDTO>.Success(dto);
        }

        public async Task<OperationResult<CategoryDTO>> UpdateAsync(UpdateCategoryRequestDTO model, CancellationToken cancellationToken = default)
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(model.id, cancellationToken);
            if (category == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (await _categoryRepository.ExistsBySlugAsync(model.slug, model.id, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            if (model.file_image != null)
            {
                await _cloudinaryService.DeleteImageAsync(category.image_public_id);

                var resultUpload = await _cloudinaryService.UploadImageAsync(model.file_image, $"Catalog/Category/{category.id}");
                if (resultUpload.IsSuccess)
                {
                    category.UpdateLogo(resultUpload.Url, resultUpload.PublicId);
                }
            }

            category.Update(model.name, model.slug, model.parent_id, model.description, model.display_order, model.is_active);

            _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            CategoryDTO dto = _mapper.Map<CategoryDTO>(category);
            return OperationResult<CategoryDTO>.Success(dto);
        }

        public async Task<OperationResult<bool>> DeleteAsync(DeleteCategoryRequestDTO model, CancellationToken cancellationToken = default)
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(model.id, cancellationToken);
            if (category == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (category.products.Any())
                throw new ConflictException("Không thể xóa danh mục đang có sản phẩm.");

            _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa danh mục thành công");
        }
        public async Task<OperationResult<bool>> UpdateActiveAsync(UpdateActiveCategoryRequestDTO model, CancellationToken cancellationToken = default)
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(model.id, cancellationToken);
            if (category == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (category.is_active) category.Deactivate(); else category.Activate();

            _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Cập nhật trạng thái thành công");
        }
        public async Task<OperationResult<CategoryDTO>> CategoryGetByIdAsync(CategoryGetByIdRequestDTO model, CancellationToken cancellationToken = default)
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(model.id, cancellationToken);
            if (category == null) throw new NotFoundException("Không tìm thấy danh mục.");
            CategoryDTO dto = _mapper.Map<CategoryDTO>(category);

            return OperationResult<CategoryDTO>.Success(dto);
        }
        public async Task<OperationResult<PagedResult<CategoryDTO>>> CategoryGetPagedAsync(GetCategoriesPagedRequestDTO model, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _categoryRepository.CategoryGetPagedAsync(
                model.page_number,
                model.page_size,
                model.search_text,
                model.sort_by,
                model.sort_dir,
                cancellationToken);

            var pagedResult = new PagedResult<Category>(items, total, model.page_number, model.page_size);
            var dtoResult = pagedResult.MapTo(_mapper.Map<CategoryDTO>);

            return OperationResult<PagedResult<CategoryDTO>>.Success(dtoResult);
        }
        #endregion
    }
}