using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на обновление пароля пользователя
    /// </summary>
    /// <param name="UserUpdate">Запрос на обновление пароля пользователя</param>
    public record UpdateUserPasswordCommand(UserUpdatePasswordRequest UserUpdate) : IRequest<Result>;

    /// <summary>
    /// Обработчик команды <see cref="UpdateUserPasswordCommand"/> на обновление пароля пользователя
    /// </summary>
    internal class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, Result>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IPasswordService"/>
        private readonly IPasswordService _passwordService;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<UpdateUserPasswordHandler> _logger;

        public UpdateUserPasswordHandler(IUserRepository repository,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ILogger<UpdateUserPasswordHandler> logger)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _repository.GetByIdAsync(request.UserUpdate.Id)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        DomainFieldCodes.DomainFieldId, request.UserUpdate.Id);

                if (!_passwordService.ValidateHash(request.UserUpdate.OldPassword, user.PasswordHash))
                    throw new DomainValidationFieldException(ServicesUsersFieldCodes.ServicesUsersFieldOldPassword);

                user.ChangePassword(_passwordService.GetHashPassword(request.UserUpdate.NewPassword));

                _repository.Update(user);

                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException dex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                return Result.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                _logger.LogError(ex, ex.Message);

                return Result.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
