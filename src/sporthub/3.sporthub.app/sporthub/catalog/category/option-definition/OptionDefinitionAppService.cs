using AutoMapper;
using Shared.Common;
using Shared.Exceptions;
using sporthub.app.contracts;
using sporthub.domain;

namespace sporthub.app
{
    public class OptionDefinitionAppService : IOptionDefinitionAppService
    {
        private readonly IOptionDefinitionRepository _optionDefinitionRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OptionDefinitionAppService(
            ISportHubUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionDefinitionRepository optionDefinitionRepository
        )
        {
            _optionDefinitionRepository = optionDefinitionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<OptionDefinitionDTO>> CreateAsync(CreateOptionDefinitionRequestDTO model, CancellationToken cancellationToken = default)
        {
            var code = model.code.Trim().ToLowerInvariant();
            if (await _optionDefinitionRepository.OptionDefinitionExistsByCodeAsync(code, null, cancellationToken))
                throw new ConflictException($"code '{code}' đã tồn tại.");

            var entity = OptionDefinition.Create(code, model.name);
            await _optionDefinitionRepository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<OptionDefinitionDTO>.Success(_mapper.Map<OptionDefinitionDTO>(entity));
        }

        public async Task<OperationResult<OptionDefinitionDTO>> UpdateAsync(UpdateOptionDefinitionRequestDTO model, CancellationToken cancellationToken = default)
        {
            var entity = await _optionDefinitionRepository.OptionDefinitionGetByIdAsync(model.id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy option.");

            entity.Update(model.name, model.is_active);
            _optionDefinitionRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<OptionDefinitionDTO>.Success(_mapper.Map<OptionDefinitionDTO>(entity));
        }

        public async Task<OperationResult<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var entity = await _optionDefinitionRepository.OptionDefinitionGetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy option.");

            // do OnDelete Restrict ở category_option và product_variant_option_value,
            // nếu còn category hoặc variant đang dùng thì SaveChanges sẽ báo lỗi FK.
            // Kiểm tra trước để trả lỗi nghiệp vụ rõ ràng thay vì để lỗi DB rơi xuống.
            await _optionDefinitionRepository.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa option thành công");
        }

        public async Task<OperationResult<List<OptionDefinitionDTO>>> GetNoPagingAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _optionDefinitionRepository.OptionDefinitionGetNoPagingAsync(cancellationToken);
            return OperationResult<List<OptionDefinitionDTO>>.Success(_mapper.Map<List<OptionDefinitionDTO>>(entities));
        }
    }
}