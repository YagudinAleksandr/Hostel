using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на сброс пароля пользователя
    /// </summary>
    /// <param name="Email">Адрес электронной почты</param>
    public record CreateUserResetPasswordTokenCommand(string Email) : IRequest<Result>;

    /// <summary>
    /// Обработчик команды <see cref="CreateUserResetPasswordTokenCommand"/> на сброс пароля пользователя
    /// </summary>
    internal class CreateUserResetPasswordTokenHandler : IRequestHandler<CreateUserResetPasswordTokenCommand, Result>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _userRepository;

        /// <inheritdoc cref="IResetUserPasswordService"/>
        private readonly IResetUserPasswordService _resetService;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<CreateUserResetPasswordTokenHandler> _logger;

        public CreateUserResetPasswordTokenHandler(IResetUserPasswordService resetService,
            IUserRepository userRepository,
            ILogger<CreateUserResetPasswordTokenHandler> logger)
        {
            _resetService = resetService;
            _logger = logger;
            _userRepository = userRepository;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result> Handle(CreateUserResetPasswordTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        ServicesUsersFieldCodes.ServicesUsersFieldEmail, request.Email);

                await _resetService.CreateResetPasswordAsync(user.Id, cancellationToken);

                return Result.Success();
            }
            catch(DomainException dex)
            {
                return Result.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
