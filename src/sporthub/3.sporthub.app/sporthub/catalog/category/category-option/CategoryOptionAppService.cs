using AutoMapper;
using Shared.Common;
using Shared.Exceptions;
using sporthub.app.contracts;
using sporthub.domain;

namespace sporthub.app
{
    public class CategoryOptionAppService : ICategoryOptionAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryOptionRepository _CategoryOptionRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryOptionAppService(
            ICategoryOptionRepository CategoryOptionRepository,
            ISportHubUnitOfWork unitOfWork,
            IMapper mapper,
            ICategoryRepository categoryRepository
        )
        {
            _CategoryOptionRepository = CategoryOptionRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<
            OperationResult<List<CategoryOptionDTO>>
        > CategoryOptionGetNoPagingAsync(
            CategoryOptionGetNoPagingRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            List<CategoryOption> category_option_maps =
                await _CategoryOptionRepository.CategoryOptionGetNoPagingAsync(
                    model.search_text,
                    model.category_id,
                    cancellationToken
                );
            List<CategoryOptionDTO> dto = _mapper.Map<List<CategoryOptionDTO>>(
                category_option_maps
            );
            return OperationResult<List<CategoryOptionDTO>>.Success(dto);
        }

        public async Task<OperationResult<CategoryOptionDTO>> CategoryOptionGetByIdAsync(
            CategoryOptionGetByIdRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            CategoryOption category_option_map =
                await _CategoryOptionRepository.CategoryOptionGetByIdAsync(
                    model.id,
                    cancellationToken
                );
            if (category_option_map == null)
                throw new NotFoundException("Không tìm thấy cấu hình option .");
            CategoryOptionDTO dto = _mapper.Map<CategoryOptionDTO>(category_option_map);

            return OperationResult<CategoryOptionDTO>.Success(dto);
        }

        public async Task<OperationResult<bool>> ReplaceAllAsync(
            ReplaceAllAsyncRequestDTO model,
            CancellationToken cancellationToken = default
        )
        {
            var category = await _categoryRepository.CategoryGetByIdAsync(model.category_id, cancellationToken)
                 ?? throw new NotFoundException("Không tìm thấy danh mục.");

            if (category.parent_id is not null)
                throw new BadRequestException("Danh mục con dùng option của danh mục cha, không khai báo riêng.");


            var duplicated = model
                .category_option_maps.GroupBy(x => x.option_column)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicated.Count > 0)
                throw new ConflictException(
                    $"option_column bị trùng: {string.Join(", ", duplicated)}"
                );

            foreach (var item in category.category_option)
            {
                var existing = await _CategoryOptionRepository.CategoryOptionGetByIdAsync(item.id, cancellationToken);
                if (existing != null)
                    await _CategoryOptionRepository.DeleteAsync(existing, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var entities = model
                .category_option_maps.Select(x =>
                    CategoryOption.Create(
                        model.category_id,
                        0,
                        x.display_order
                    )
                )
                .ToList();
            foreach (var item in model.category_option_maps)
            {
                var categoryOption = CategoryOption.Create(model.category_id, 0, item.display_order);
                await _CategoryOptionRepository.CreateAsync(categoryOption, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult<bool>.Success(true, "Cập nhật cấu hình option thành công");
        }
    }
}
