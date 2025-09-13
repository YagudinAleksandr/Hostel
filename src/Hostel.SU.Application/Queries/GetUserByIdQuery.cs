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
    /// Запрос на получение пользователя по идентификатору
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя</param>
    public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserResponse>>;

    /// <summary>
    /// Обработчик запроса <see cref="GetUserByIdQuery"/> на получение пользователя по идентификатору
    /// </summary>
    internal class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<GetUserByIdHandler> _logger;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository repository, IMapper mapper, ILogger<GetUserByIdHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdAsync(request.UserId, cancellationToken)
                    ?? throw new DomainResourceNotFoundException(ServicesUsersEntitiesCodes.ServicesUsersEntityUser,
                        DomainFieldCodes.DomainFieldId, request.UserId);

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
