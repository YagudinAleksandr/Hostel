using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.Users.Contracts.Request;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на сброс пароля по токену
    /// </summary>
    /// <param name="UserReset">Данные для смены пароля</param>
    public record UpdateResetTokenPasswordCommand(UserResetPasswordRequest UserReset) : IRequest<Result>;

    /// <summary>
    /// Обработчик команды <see cref="UpdateResetTokenPasswordCommand"/> на сброс пароля по токену
    /// </summary>
    internal class UpdateResetTokenPasswordHandler : IRequestHandler<UpdateResetTokenPasswordCommand, Result>
    {
        #region CTOR
        /// <inheritdoc cref="IResetUserPasswordService"/>
        private readonly IResetUserPasswordService _resetUserPasswordService;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<UpdateResetTokenPasswordHandler> _logger;

        public UpdateResetTokenPasswordHandler(IResetUserPasswordService resetUserPasswordService, ILogger<UpdateResetTokenPasswordHandler> logger)
        {
            _resetUserPasswordService = resetUserPasswordService;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result> Handle(UpdateResetTokenPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _resetUserPasswordService.ChangePasswordAsync(request.UserReset.Token, request.UserReset.Password, cancellationToken);

                return Result.Success();
            }
            catch (DomainException dex)
            {
                return Result.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);

                return Result.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
