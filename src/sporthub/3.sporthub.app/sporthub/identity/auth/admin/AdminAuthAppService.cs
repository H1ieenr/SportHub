using Microsoft.AspNetCore.Identity;
using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using Shared.Exceptions;
using Microsoft.Extensions.Options;

namespace sporthub.app
{
    public class AdminAuthAppService : IAdminAuthAppService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly JwtSettings _settings;
        public AdminAuthAppService(IUsersRepository usersRepository, IRefreshTokensRepository refreshTokensRepository,
            IJwtTokenService jwtTokenService, IPasswordHasher<Users> passwordHasher, ISportHubUnitOfWork unitOfWork,
            IRolesRepository rolesRepository, IOptions<JwtSettings> options)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _settings = options.Value;
        }

        public async Task<OperationResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO model, CancellationToken cancellationToken = default)
        {
            var email = model.email.Trim().ToLowerInvariant();

            var user = await _usersRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null || user.status != UserStatus.Active)
                throw new ConflictException("Email hoặc mật khẩu không chính xác.");

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.password_hash, model.password);
            if (verifyResult == PasswordVerificationResult.Failed)
                throw new ConflictException("Email hoặc mật khẩu không chính xác.");

            var roles = user.user_roles.Select(x => x.roles.name).ToList();
            if (!roles.Any(r => r == "Admin" || r == "Staff"))
                throw new ForbiddenException("Tài khoản không có quyền truy cập.");

            var accessTokenResult = _jwtTokenService.CreateAccessToken(
            new CreateAccessTokenRequestDTO
            {
                user = new UsersDTO { id = user.id, email = user.email, name = user.name, roles = roles },
                audience = _settings.AdminAudience
            });

            var rawRefreshToken = _jwtTokenService.CreateRefreshToken();
            var hashRefreshToken = _jwtTokenService.HashRefreshToken(new HashRefreshTokenRequestDTO
            {
                rawToken = rawRefreshToken.refresh_token
            });
            await _refreshTokensRepository.AddAsync(
                new RefreshTokens(
                    user.id,
                    _settings.AdminAudience,
                    hashRefreshToken.hash_refresh_token,
                    _jwtTokenService.GetRefreshTokenExpiry().refresh_token_expiry), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new LoginResponseDTO
            {
                access_token = accessTokenResult.access_token,
                access_token_expires_at = accessTokenResult.expires_at,
                refresh_token = rawRefreshToken.refresh_token,
                user = new UsersDTO
                {
                    id = user.id,
                    email = user.email,
                    name = user.name,
                    phone = user.phone,
                    avatar_url = user.avatar_url,
                    status = (UserStatusDTO)user.status,
                    roles = roles
                }
            };
            return OperationResult<LoginResponseDTO>.Success(response);
        }
    }
}