using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на удаление пользователя
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя</param>
    public record DeleteUserCommand(Guid UserId) :  IRequest<Result>;

    /// <summary>
    /// Обработчик команды <see cref="DeleteUserCommand"/> на удаление пользователя
    /// </summary>
    internal class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(IUserRepository repository, IUnitOfWork unitOfWork, ILogger<DeleteUserHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _repository.GetByIdAsync(request.UserId, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        DomainFieldCodes.DomainFieldId, request.UserId);

                _repository.Delete(user);

                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch(DomainException dex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                return Result.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                _logger.LogError(ex, ex.Message);

                return Result.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
