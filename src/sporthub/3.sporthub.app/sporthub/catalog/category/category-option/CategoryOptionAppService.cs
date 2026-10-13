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
        private readonly IOptionDefinitionRepository _optionDefinitionRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryOptionAppService(
            ICategoryOptionRepository CategoryOptionRepository,
            ISportHubUnitOfWork unitOfWork,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IOptionDefinitionRepository optionDefinitionRepository  
        )
        {
            _CategoryOptionRepository = CategoryOptionRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _optionDefinitionRepository = optionDefinitionRepository;
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

        public async Task<OperationResult<bool>> ReplaceAllAsync(ReplaceAllAsyncRequestDTO model, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.CategoryGetByIdAsync(model.category_id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy danh mục.");

            if (category.parent_id is not null)
                throw new BadRequestException("Danh mục con dùng option của danh mục cha, không khai báo riêng.");

            var definitionIds = model.category_options.Select(x => x.option_definition_id).ToList();

            var duplicated = definitionIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicated.Count > 0)
                throw new ConflictException($"option_definition_id bị trùng: {string.Join(", ", duplicated)}");

            if (definitionIds.Count > 0)
            {
                var existingDefIds = await _optionDefinitionRepository.OptionDefinitionExistingIdsAsync(definitionIds, cancellationToken);
                var missing = definitionIds.Except(existingDefIds).ToList();
                if (missing.Count > 0)
                    throw new NotFoundException($"option_definition_id không tồn tại: {string.Join(", ", missing)}");
            }

            var current = await _CategoryOptionRepository.CategoryOptionGetTrackedByCategoryIdAsync(category.id, cancellationToken);

            foreach (var item in model.category_options)
            {
                var found = current.FirstOrDefault(x => x.option_definition_id == item.option_definition_id);
                if (found != null)
                {
                    found.Update(category.id, item.display_order);
                    await _CategoryOptionRepository.UpdateAsync(found, cancellationToken);
                }
                else
                {
                    var entity = CategoryOption.Create(category.id, item.option_definition_id, item.display_order);
                    await _CategoryOptionRepository.CreateAsync(entity, cancellationToken);
                }
            }

            var toRemove = current.Where(x => !definitionIds.Contains(x.option_definition_id)).ToList();
            foreach (var r in toRemove)
                await _CategoryOptionRepository.DeleteAsync(r, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult<bool>.Success(true, "Cập nhật cấu hình option thành công");
        }
    }
}
