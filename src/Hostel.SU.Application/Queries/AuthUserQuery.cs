using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.Users.Contracts.Request;
using Hostel.Users.Contracts.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Запрос на авторизацию пользователя
    /// </summary>
    /// <param name="Login">Запрос на авторизацию</param>
    public record AuthUserQuery(UserLoginRequest Login) : IRequest<Result<UserLoginResponse>>;

    /// <summary>
    /// Обработчик запроса <see cref="AuthUserQuery"/> на авторизацию
    /// </summary>
    internal class AuthUserHandler : IRequestHandler<AuthUserQuery, Result<UserLoginResponse>>
    {
        #region CTOR
        /// <inheritdoc cref="IAuthService"/>
        private readonly IAuthService _authService;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<AuthUserHandler> _logger;

        public AuthUserHandler(IAuthService authService, ILogger<AuthUserHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<UserLoginResponse>> Handle(AuthUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.LoginAsync(request.Login.Email, request.Login.Password);

                return Result<UserLoginResponse>.Success(result);
            }
            catch (DomainException dex)
            {
                return Result<UserLoginResponse>.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);

                return Result<UserLoginResponse>.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
