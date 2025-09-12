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
    /// Команда обновления адреса электронной почты
    /// </summary>
    /// <param name="UserUpdate">Запрос на обновление адреса жоектронной почты</param>
    public record UpdateUserEmailCommand(UserUpdateEmailRequest UserUpdate) : IRequest<Result<UserResponse>>;

    /// <summary>
    /// Обработчик аоманды <see cref="UpdateUserEmailCommand"/> на обновление адреса электронной почты
    /// </summary>
    public class UpdateUserEmailHandler : IRequestHandler<UpdateUserEmailCommand, Result<UserResponse>>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<UpdateUserEmailHandler> _logger;

        public UpdateUserEmailHandler(IUserRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateUserEmailHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<UserResponse>> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _repository.GetByIdAsync(request.UserUpdate.Id, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                    DomainFieldCodes.DomainFieldId,
                    request.UserUpdate.Id);

                if (await _repository.IsEmailExists(request.UserUpdate.Email, user.Id))
                    throw new DomainResourceAlreadyExistsException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        ServicesUsersFieldCodes.ServicesUsersFieldEmail, request.UserUpdate.Email);

                user.ChangeEmail(new EmailVo(request.UserUpdate.Email));

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
