using AutoMapper;
using Shared.Common;
using Shared.Exceptions;
using sporthub.app.contracts;
using sporthub.domain;

namespace sporthub.app
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryOptionRepository _categoryOptionRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ICategoryOptionAppService _categoryOptionAppService;
        private readonly IMapper _mapper;
        private readonly string _folderName = "Catalog/Category/";

        public CategoryAppService(
            ICategoryRepository categoryRepository,
            ICategoryOptionRepository categoryOptionRepository,
            ISportHubUnitOfWork unitOfWork,
            ICloudinaryService cloudinaryService,
            IMapper mapper,
            ICategoryOptionAppService categoryOptionAppService
        )
        {
            _categoryRepository = categoryRepository;
            _categoryOptionRepository = categoryOptionRepository;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _categoryOptionAppService = categoryOptionAppService;
        }

        #region admin
        public async Task<OperationResult<CategoryDTO>> CreateAsync(CreateCategoryRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (await _categoryRepository.ExistsBySlugAsync(model.slug, null, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");
            if (model.parent_id is not null && model.category_options.Count > 0)
                throw new BadRequestException("Danh mục con không được khai báo option riêng.");

            var duplicated = model.category_options
                .GroupBy(x => x.option_definition_id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicated.Count > 0)
                throw new ConflictException($"option_definition_id bị trùng: {string.Join(", ", duplicated)}");

            Category category = Category.Create(model.name, model.slug, model.parent_id, model.description, model.display_order, model.is_active);

            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (model.category_options.Count > 0)
            {
                await _categoryOptionAppService.ReplaceAllAsync(
                    new ReplaceAllAsyncRequestDTO
                    {
                        category_id = category.id,
                        category_options = model.category_options
                    },
                    cancellationToken
                );
            }

            if (model.file_image != null)
            {
                var resultUpload = await _cloudinaryService.UploadImageAsync(model.file_image, $"{_folderName}{category.id}");
                if (resultUpload.IsSuccess)
                {
                    category.UpdateLogo(resultUpload.Url, resultUpload.PublicId);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            return OperationResult<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category));
        }

        public async Task<OperationResult<CategoryDTO>> UpdateAsync(UpdateCategoryRequestDTO model, CancellationToken cancellationToken = default)
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(model.id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy danh mục.");

            if (await _categoryRepository.ExistsBySlugAsync(model.slug, model.id, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            if (model.parent_id is not null && model.category_options?.Count > 0)
                throw new BadRequestException("Danh mục con không được khai báo option riêng.");

            if (model.parent_id is not null && category.parent_id is null)
            {
                var hasOptions = (await _categoryOptionRepository.CategoryOptionGetNoPagingAsync(null, category.id, cancellationToken)).Count > 0;
                if (hasOptions)
                    throw new ConflictException("Danh mục đang có option, không thể chuyển thành danh mục con.");
            }

            if (model.category_options is not null)
            {
                await _categoryOptionAppService.ReplaceAllAsync(new ReplaceAllAsyncRequestDTO
                {
                    category_id = category.id,
                    category_options = model.category_options
                }, cancellationToken);
            }

            if (model.file_image != null)
            {
                await _cloudinaryService.DeleteImageAsync(category.image_public_id);

                var resultUpload = await _cloudinaryService.UploadImageAsync(model.file_image, $"{_folderName}{category.id}");
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

        public async Task<OperationResult<bool>> DeleteAsync(
            DeleteCategoryRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(
                model.id,
                cancellationToken
            );
            if (category == null)
                throw new NotFoundException("Không tìm thấy danh mục.");

            if (category.products.Any())
                throw new ConflictException("Không thể xóa danh mục đang có sản phẩm.");

            _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa danh mục thành công");
        }

        public async Task<OperationResult<bool>> UpdateActiveAsync(
            UpdateActiveCategoryRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(
                model.id,
                cancellationToken
            );
            if (category == null)
                throw new NotFoundException("Không tìm thấy danh mục.");

            if (category.is_active)
                category.Deactivate();
            else
                category.Activate();

            _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Cập nhật trạng thái thành công");
        }

        public async Task<OperationResult<CategoryDTO>> CategoryGetByIdAsync(
            CategoryGetByIdRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            Category category = await _categoryRepository.CategoryGetByIdAsync(model.id, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy danh mục.");

            CategoryDTO dto = _mapper.Map<CategoryDTO>(category);

            var effectiveCategoryId = category.parent_id ?? category.id;
            var options = effectiveCategoryId == category.id
                ? category.category_option
                : await _categoryOptionRepository.CategoryOptionGetNoPagingAsync(null, effectiveCategoryId, cancellationToken);

            dto.category_options = _mapper.Map<List<CategoryOptionDTO>>(options);
            return OperationResult<CategoryDTO>.Success(dto);
        }

        public async Task<OperationResult<PagedResult<CategoryDTO>>> CategoryGetPagedAsync(
            GetCategoriesPagedRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            var (items, total) = await _categoryRepository.CategoryGetPagedAsync(
                model.page_number,
                model.page_size,
                model.search_text,
                model.sort_by,
                model.sort_dir,
                model.active,
                model.parent_id,
                cancellationToken
            );

            var pagedResult = new PagedResult<Category>(
                items,
                total,
                model.page_number,
                model.page_size
            );
            var dtoResult = pagedResult.MapTo(_mapper.Map<CategoryDTO>);

            return OperationResult<PagedResult<CategoryDTO>>.Success(dtoResult);
        }
        #endregion
        public async Task<OperationResult<List<CategoryDTO>>> CategoryGetNoPagingAsync(
            GetCategoriesNoPagingRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            var items = await _categoryRepository.CategoryGetNoPagingAsync(
                model.search_text,
                model.active,
                model.parent_id,
                cancellationToken
            );
            List<CategoryDTO> dto = _mapper.Map<List<CategoryDTO>>(items);
            return OperationResult<List<CategoryDTO>>.Success(dto);
        }
    }
}
