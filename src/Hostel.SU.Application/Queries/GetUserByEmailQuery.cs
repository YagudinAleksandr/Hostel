using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Запрос на получение пользователя по адресу электронной почты
    /// </summary>
    /// <param name="Email">Адрес электронной почты</param>
    public record GetUserByEmailQuery(string Email) : IRequest<Result<UserResponse>>;

    internal class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, Result<UserResponse>>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<GetUserByEmailHandler> _logger;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        public GetUserByEmailHandler(IUserRepository repository, IMapper mapper, ILogger<GetUserByEmailHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByEmailAsync(request.Email, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        ServicesUsersFieldCodes.ServicesUsersFieldEmail, request.Email);

                return Result<UserResponse>.Success(_mapper.Map<UserResponse>(user));
            }
            catch (DomainException dex)
            {
                return Result<UserResponse>.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result<UserResponse>.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
