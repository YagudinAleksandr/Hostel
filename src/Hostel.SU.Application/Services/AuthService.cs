using AutoMapper;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using System.Threading;

namespace Hostel.SU.Application
{
    /// <inheritdoc/>
    internal class AuthService : IAuthService
    {
        #region CTOR

        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _userRepository;

        /// <inheritdoc cref="IRefreshTokenRepository"/>
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        /// <inheritdoc cref="ITokenGeneratorService"/>
        private readonly ITokenGeneratorService _tokenGeneratorService;

        /// <inheritdoc cref="IPasswordService"/>
        private readonly IPasswordService _passwordService;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenGeneratorService tokenGeneratorService,
            IPasswordService passwordService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenGeneratorService = tokenGeneratorService;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        #endregion

        /// <inheritdoc/>
        public async Task<bool> IsTokenRevokedAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            return token == null || !token.IsActive;
        }

        /// <inheritdoc/>
        public async Task<UserLoginResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken)
                ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                    ServicesUsersFieldCodes.ServicesUsersFieldEmail,
                    email);

            if (user.Status == UserStatuses.Inactive || user.Status == UserStatuses.Blocked)
                throw new DomainInactiveUserException();

            if (!_passwordService.ValidateHash(password, user.PasswordHash))
                throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser);

            string accessToken = _tokenGeneratorService.GenerateAccessToken(user.Id, user.Type);
            string refreshToken = await SaveNewGeneratedRefreshToken(user.Id, cancellationToken);

            return new UserLoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserResponse>(user)
            };
        }

        /// <inheritdoc/>
        public async Task<UserLoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (storedToken == null || !storedToken.IsActive)
                throw new DomainExpiredTokenException();

            var user = await _userRepository.GetByIdAsync(storedToken.UserId, cancellationToken)
                ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser);

            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);

            string accessToken = _tokenGeneratorService.GenerateAccessToken(user.Id, user.Type);
            refreshToken = await SaveNewGeneratedRefreshToken(user.Id, cancellationToken);

            return new UserLoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserResponse>(user)
            };
        }

        /// <inheritdoc/>
        public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken)
                ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityRefreshToken);

            token.Revoke();
            await _refreshTokenRepository.UpdateAsync(token);
        }

        /// <summary>
        /// Сохраняет новый токен обновления
        /// </summary>
        /// <param name="userId">Идентификаор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Сгенерированный токен обновления</returns>
        /// <remarks>Генерирует новый токен обновления из <paramref name="userId"/> и сохраняет</remarks>
        private async Task<string> SaveNewGeneratedRefreshToken(Guid userId, CancellationToken cancellationToken)
        {
            var refreshToken = _tokenGeneratorService.GenerateRefreshToken(userId);

            var refreshTokenEntity = new RefreshToken(refreshToken,
                DateTime.UtcNow.AddMinutes(_tokenGeneratorService.RefreshTokenLifetimeMinutes),
                userId);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            return refreshToken;
        }
    }
}
