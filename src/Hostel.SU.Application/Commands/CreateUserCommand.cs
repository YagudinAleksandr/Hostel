using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Request;
using Hostel.Users.Contracts.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using Hostel.Domain.Primitives;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Команда на создание пользователя
    /// </summary>
    /// <param name="UserCreate">Запрос на создание пользователя</param>
    public record CreateUserCommand(UserCreateRequest UserCreate) : IRequest<Result<UserResponse>>;

    /// <summary>
    /// Обработчик команды <see cref="CreateUserCommand"/> на создание пользователя
    /// </summary>
    internal class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
    {
        #region CTOR

        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IUnitOfWork"/>
        private readonly IUnitOfWork _unitOfWork;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        /// <inheritdoc cref="IPasswordService"/>
        private readonly IPasswordService _passwordService;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IUserRepository repository,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            IMapper mapper,
            ILogger<CreateUserHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _passwordService = passwordService;
        }

        #endregion
        public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var createUser = request.UserCreate;

            try
            {
                if (await _repository.IsEmailExistsAsync(createUser.Email, cancellationToken: cancellationToken))
                    throw new DomainResourceAlreadyExistsException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        ServicesUsersFieldCodes.ServicesUsersFieldEmail,
                        createUser.Email);

                var userType = UserTypes.All.FirstOrDefault(x => x.Code.Equals(createUser.Type, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new DomainRequiredFieldException(ServicesUsersFieldCodes.ServicesUsersFieldType);

                var userStatus = UserStatuses.All.FirstOrDefault(x => x.Code.Equals(createUser.Status, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new DomainRequiredFieldException(ServicesUsersFieldCodes.ServicesUsersFieldStatus);

                var user = User.Create(new EmailVo(createUser.Email),
                    new FullNameVo(createUser.Firstname, createUser.Lastname, createUser.Patronymic),
                    _passwordService.GetHashPassword(createUser.Password),
                    userType,
                    userStatus);

                _repository.Add(user);

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
