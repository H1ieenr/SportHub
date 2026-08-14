using Microsoft.AspNetCore.Identity;
using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using Shared.Exceptions;
namespace sporthub.app
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly ISportHubUnitOfWork _unitOfWork;

        public AuthAppService(IUsersRepository usersRepository, IRefreshTokensRepository refreshTokensRepository,
            IJwtTokenService jwtTokenService, IPasswordHasher<Users> passwordHasher, ISportHubUnitOfWork unitOfWork,
            IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
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

            var accessTokenResult = _jwtTokenService.CreateAccessToken(
                new CreateAccessTokenRequestDTO
                {
                    user = new UsersDTO
                    {
                        id = user.id,
                        email = user.email,
                        name = user.name,
                        roles = roles
                    }
                });

            var rawRefreshToken = _jwtTokenService.CreateRefreshToken();
            var hashRefreshToken = _jwtTokenService.HashRefreshToken(new HashRefreshTokenRequestDTO
            {
                rawToken = rawRefreshToken.refresh_token
            });
            await _refreshTokensRepository.AddAsync(
                new RefreshTokens(
                    user.id,
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

        public async Task<OperationResult<UsersDTO>> RegisterAsync(RegisterRequestDTO model, CancellationToken cancellationToken = default)
        {
            var email = model.email.Trim().ToLowerInvariant();
            var phone = model.phone.Trim();

            if (await _usersRepository.ExistsByEmailAsync(email, cancellationToken))
                throw new ConflictException("Email đã được sử dụng.");

            if (await _usersRepository.ExistsByPhoneAsync(phone, cancellationToken))
                throw new ConflictException("Số điện thoại đã được sử dụng.");

            var customerRole = await _rolesRepository.GetByNameAsync("Customer", cancellationToken);

            if (customerRole is null)
                throw new NotFoundException("Role Customer chưa được khởi tạo trong database.");

            var passwordHash = _passwordHasher.HashPassword(null!, model.password);
            var user = Users.Create(email, passwordHash, model.name.Trim(), phone);

            user.AddRole(customerRole.id);

            await _usersRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UsersDTO
            {
                id = user.id,
                email = user.email,
                name = user.name,
                phone = user.phone,
                avatar_url = user.avatar_url,
                status = (UserStatusDTO)user.status,
                roles = ["Customer"]
            };

            return OperationResult<UsersDTO>.Success(response);
        }
    }
}