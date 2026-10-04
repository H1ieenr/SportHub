using AutoMapper;
using Shared.Common;
using Shared.Exceptions;
using sporthub.app.contracts;
using sporthub.domain;

namespace sporthub.app
{
    public class OptionValueAppService : IOptionValueAppService
    {
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly IOptionDefinitionRepository _optionDefinitionRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OptionValueAppService(
            ISportHubUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionValueRepository optionValueRepository,
            IOptionDefinitionRepository optionDefinitionRepository
        )
        {
            _optionValueRepository = optionValueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _optionDefinitionRepository = optionDefinitionRepository;
        }

        public async Task<OperationResult<List<CreateOptionValueResponseDTO>>> CreateValueAsync(CreateOptionValueRequestDTO model, CancellationToken cancellationToken = default)
        {
            var definition = await _optionDefinitionRepository.OptionDefinitionGetByIdAsync(model.option_definition_id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy option.");

            var items = model.create_option_values;
            if (items.Count == 0) return OperationResult<List<CreateOptionValueResponseDTO>>.Success([]);

            var values = items.Select(x => x.value).ToList();
            var duplicated = values.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToHashSet();
            var existing = (await _optionValueRepository.OptionValuesExistsAsync(definition.id, values, cancellationToken)).ToHashSet();

            string? GetError(string value) => duplicated.Contains(value) ? "Value bị trùng trong danh sách gửi lên" : existing.Contains(value) ? $"Value đã tồn tại trong [{definition.name}]" : null;

            var rows = items.Select(item =>
            {
                var error = GetError(item.value);
                var entity = error is null ? OptionValue.Create(model.option_definition_id, item.value, item.display_order) : null;
                var dto = error is null ? CreateOptionValueResponseDTO.Ok(item.value) : CreateOptionValueResponseDTO.Fail(item.value, error);

                return (entity, dto);
            }).ToList();

            var entities = rows.Where(x => x.entity is not null).Select(x => x.entity!).ToList();
            if (entities.Count > 0)
            {
                await _optionValueRepository.CreateBatchAsync(entities);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            foreach (var (entity, dto) in rows) if (entity is not null) dto.id = entity.id;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult<List<CreateOptionValueResponseDTO>>.Success(rows.Select(x => x.dto).ToList());
        }

        public async Task<OperationResult<OptionValueDTO>> UpdateValueAsync(UpdateOptionValueRequestDTO model, CancellationToken cancellationToken = default)
        {
            var entity = await _optionValueRepository.OptionValueGetByIdAsync(model.id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy giá trị.");

            entity.Update(model.value, model.display_order);
            _optionValueRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<OptionValueDTO>.Success(_mapper.Map<OptionValueDTO>(entity));
        }

        public async Task<OperationResult<bool>> DeleteValueAsync(DeleteOptionValueRequestDTO model, CancellationToken cancellationToken = default)
        {
            var entity = await _optionValueRepository.OptionValueGetByIdAsync(model.id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy giá trị.");

            if (await _optionValueRepository.OptionValueIsUsedByAnyVariantAsync(model.id))
                throw new ConflictException("Giá trị đang được dùng bởi biến thể sản phẩm, không thể xóa.");

            await _optionValueRepository.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa giá trị thành công");
        }
    }
}