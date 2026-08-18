using Microsoft.AspNetCore.Identity;
using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using Shared.Exceptions;
using System.Threading.Tasks;
using System;

namespace sporthub.app
{
    public class AuthCommonAppService : IAuthCommonAppService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly ISportHubUnitOfWork _unitOfWork;
        public AuthCommonAppService(IUsersRepository usersRepository, IRefreshTokensRepository refreshTokensRepository,
            IJwtTokenService jwtTokenService, ISportHubUnitOfWork unitOfWork, IPasswordHasher<Users> passwordHasher,
            IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<bool>> LogoutAsync(LogoutRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(model.refresh_token))
                throw new ValidationException("Refresh token không hợp lệ.");

            var tokenHash = _jwtTokenService.HashRefreshToken(new HashRefreshTokenRequestDTO { rawToken = model.refresh_token });

            var refreshToken = await _refreshTokensRepository
                .GetActiveByTokenHashAsync(tokenHash.hash_refresh_token, cancellationToken);

            if (refreshToken is null)
            {
                return OperationResult<bool>.Success(true, "Đăng xuất thành công.");
            }

            refreshToken.UpdateRevokedAt(DateTime.Now);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Đăng xuất thành công.");
        }

        public async Task<OperationResult<LoginResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(model.refresh_token))
                throw new ValidationException("Refresh token không hợp lệ.");

            var tokenHash = _jwtTokenService.HashRefreshToken(new HashRefreshTokenRequestDTO { rawToken = model.refresh_token });

            var oldToken = await _refreshTokensRepository.GetActiveByTokenHashAsync(tokenHash.hash_refresh_token, cancellationToken);

            if (oldToken is null || oldToken.user.status != UserStatus.Active)
                throw new ForbiddenException("Refresh token không hợp lệ hoặc đã hết hạn.");

            var roles = oldToken.user.user_roles.Select(x => x.roles.name).ToList();

            var accessToken = _jwtTokenService.CreateAccessToken(
                new CreateAccessTokenRequestDTO
                {
                    user = new UsersDTO
                    {
                        id = oldToken.user.id,
                        email = oldToken.user.email,
                        name = oldToken.user.name,
                        roles = roles
                    },
                    audience = oldToken.audience
                });

            var rawRefreshToken = _jwtTokenService.CreateRefreshToken();

            oldToken.UpdateRevokedAt(DateTime.Now);

            await _refreshTokensRepository.AddAsync(
                new RefreshTokens(oldToken.user_id,
                    oldToken.audience,
                    _jwtTokenService.HashRefreshToken(new HashRefreshTokenRequestDTO { rawToken = rawRefreshToken.refresh_token }).hash_refresh_token,
                    _jwtTokenService.GetRefreshTokenExpiry().refresh_token_expiry),
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<LoginResponseDTO>.Success(
                new LoginResponseDTO
                {
                    access_token = accessToken.access_token,
                    access_token_expires_at = accessToken.expires_at,
                    refresh_token = rawRefreshToken.refresh_token,
                    user = new UsersDTO
                    {
                        id = oldToken.user.id,
                        email = oldToken.user.name,
                        name = oldToken.user.name,
                        phone = oldToken.user.phone,
                        avatar_url = oldToken.user.avatar_url,
                        status = (UserStatusDTO)oldToken.user.status,
                        roles = roles
                    }
                });
        }
        public async Task<OperationResult<bool>> ChangePasswordAsync(ChangePasswordRequestDTO model, CancellationToken cancellationToken = default)
        {
            var user = await _usersRepository.GetByIdAsync(model.user_id, cancellationToken);
            if (user == null) throw new NotFoundException();

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.password_hash, model.old_password);
            if (verifyResult == PasswordVerificationResult.Failed)
                throw new ValidationException("Mật khẩu cũ không đúng.");

            var newPasswordHash = _passwordHasher.HashPassword(user, model.new_password);
            user.ChangePassword(newPasswordHash);

            await _usersRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Đổi mật khẩu thành công");
        }
    }
}