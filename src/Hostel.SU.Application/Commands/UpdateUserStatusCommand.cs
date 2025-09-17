using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using Hostel.Users.Contracts.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на обновление статуса пользователя
    /// </summary>
    /// <param name="UserUpdate">Запрос на обновление статуса пользователя</param>
    public record UpdateUserStatusCommand(UserUpdateStatusRequest UserUpdate) : IRequest<Result<UserResponse>>;

    /// <summary>
    /// Обработчик команды <see cref="UpdateUserStatusCommand"/> на обновление статуса пользователя
    /// </summary>
    internal class UpdateUserStatusHandler : IRequestHandler<UpdateUserStatusCommand, Result<UserResponse>>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<UpdateUserStatusHandler> _logger;

        public UpdateUserStatusHandler(IUserRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UpdateUserStatusHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<UserResponse>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var updateUser = request.UserUpdate;

            try
            {
                var user = await _repository.GetByIdAsync(updateUser.Id)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        ServicesUsersFieldCodes.ServicesUsersFieldType, updateUser.Id);

                var userStatus = UserStatuses.All.FirstOrDefault(x => x.Code.Equals(updateUser.StatusCode, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new DomainRequiredFieldException(ServicesUsersFieldCodes.ServicesUsersFieldStatus);

                user.ChangeStatus(userStatus);

                _repository.Update(user);

                await _unitOfWork.CommitAsync(cancellationToken);

                return Result<UserResponse>.Success(_mapper.Map<UserResponse>(user));
            }
            catch (DomainException dex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                return Result<UserResponse>.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);

                _logger.LogError(ex, ex.Message);

                return Result<UserResponse>.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
