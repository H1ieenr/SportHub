using Microsoft.AspNetCore.Identity;
using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;
using System;

namespace sporthub.app
{
    public class UsersAppService : IUsersAppService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersAppService(IUsersRepository usersRepository, ISportHubUnitOfWork unitOfWork, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<GetByIdAsyncDTO>> GetByIdAsync(GetByIdAsyncRquestDTO model, CancellationToken cancellationToken = default)
        {
            var repoResult = await _usersRepository.GetByIdAsync(model.id, cancellationToken);
            if (repoResult == null) throw new NotFoundException();
            var dto = _mapper.Map<GetByIdAsyncDTO>(repoResult);
            dto.roles = repoResult.user_roles.Select(x => x.roles.name).ToList();

            return OperationResult<GetByIdAsyncDTO>.Success(dto);
        }
        // // ─── 2. CREATE ───────────────────────────────────────────────────
        // public async Task<OperationResult<UserDetailDTO>> CreateAsync(CreateUserRequestDTO model, CancellationToken cancellationToken = default)
        // {
        //     // Check nghiệp vụ
        //     var isEmailExist = await _usersRepository.ExistsByEmailAsync(model.Email, cancellationToken);
        //     if (isEmailExist)
        //         throw new ConflictException("Email đã tồn tại trong hệ thống.");

        //     // Gọi Factory Method của Domain
        //     var user = User.Create(model.Email, model.FullName);

        //     await _usersRepository.AddAsync(user, cancellationToken);
        //     await _unitOfWork.SaveChangesAsync(cancellationToken);

        //     var dto = _mapper.Map<UserDetailDTO>(user);
        //     return OperationResult<UserDetailDTO>.Created(dto, "Tạo người dùng thành công.");
        // }

        // ─── 3. UPDATE ───────────────────────────────────────────────────
        // public async Task<OperationResult<UserDetailDTO>> UpdateAsync(Guid id, UpdateUserRequestDTO model, CancellationToken cancellationToken = default)
        // {
        //     var user = await _usersRepository.GetByIdAsync(id, cancellationToken);
        //     if (user == null || user.IsDeleted)
        //         throw new NotFoundException("Không tìm thấy người dùng.");

        //     // Gọi Domain Method để thay đổi trạng thái
        //     user.UpdateProfile(model.FullName);

        //     _usersRepository.Update(user);
        //     await _unitOfWork.SaveChangesAsync(cancellationToken);

        //     var dto = _mapper.Map<UserDetailDTO>(user);
        //     return OperationResult<UserDetailDTO>.Updated(dto, "Cập nhật người dùng thành công.");
        // }

        // // ─── 4. DELETE ───────────────────────────────────────────────────
        // public async Task<OperationResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        // {
        //     var user = await _usersRepository.GetByIdAsync(id, cancellationToken);
        //     if (user == null || user.IsDeleted)
        //         throw new NotFoundException("Không tìm thấy người dùng.");

        //     // Gọi Domain Method xóa mềm
        //     user.SoftDelete();

        //     _usersRepository.Update(user);
        //     await _unitOfWork.SaveChangesAsync(cancellationToken);

        //     return OperationResult<bool>.Deleted("Xóa người dùng thành công.");
        // }
    }
}