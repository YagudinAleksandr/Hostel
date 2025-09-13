using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using Hostel.Users.Contracts.Response;
using Hostel.Domain.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на обновление ФИО пользователя
    /// </summary>
    /// <param name="UserUpdate">Запрос на обновление ФИО</param>
    public record UpdateUserFullnameCommand(UserUpdateFullnameRequest UserUpdate) : IRequest<Result<UserResponse>>;

    /// <summary>
    /// Обработчик команды <see cref="UpdateUserFullnameCommand"/> на обновление ФИО пользователя
    /// </summary>
    internal class UpdateUserFullnameHandler : IRequestHandler<UpdateUserFullnameCommand, Result<UserResponse>>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<UpdateUserFullnameHandler> _logger;

        public UpdateUserFullnameHandler(IUserRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateUserFullnameHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<UserResponse>> Handle(UpdateUserFullnameCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = await _repository.GetByIdAsync(request.UserUpdate.Id, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                    DomainFieldCodes.DomainFieldId,
                    request.UserUpdate.Id);

                user.ChangeName(new FullNameVo(request.UserUpdate.Firstname, request.UserUpdate.Lastname, request.UserUpdate.Patronymic));

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
