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
    /// Запрос на получение всех пользователей
    /// </summary>
    /// <param name="Filter">Фильтр для выборки</param>
    public record GetAllUsersQuery(UnifiedFilter Filter) : IRequest<Result<PagedResult<UserResponse>>>;

    /// <summary>
    /// Обработчик запроса <see cref="GetAllUsersQuery"/> на получение всех пользователей
    /// </summary>
    internal class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<PagedResult<UserResponse>>>
    {
        #region CTOR
        /// <inheritdoc cref="IUserRepository"/>
        private readonly IUserRepository _repository;

        /// <inheritdoc cref="IMapper"/>
        private readonly IMapper _mapper;

        /// <inheritdoc cref="ILogger"/>
        private readonly ILogger<GetAllUsersHandler> _logger;

        public GetAllUsersHandler(IUserRepository repository, IMapper mapper, ILogger<GetAllUsersHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <inheritdoc/>
        public async Task<Result<PagedResult<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userQuery = await _repository.GetAllAsync(cancellationToken);

                if (userQuery == null)
                    return Result<PagedResult<UserResponse>>.Success(new PagedResult<UserResponse>(Array.Empty<UserResponse>(), 0, 0, 0));

                var totalCount = userQuery.Count();

                if (request.Filter.SortOptions.Any())
                {
                    foreach (var sortOption in request.Filter.SortOptions)
                    {
                        userQuery = userQuery.OrderByDynamic(
                            sortOption.Field,
                            sortOption.Direction.ToLower() == "asc"
                        );
                    }
                }
                else
                {
                    userQuery = userQuery.OrderBy(r => r.Name);
                }

                userQuery = userQuery
                    .Skip(request.Filter.Skip)
                    .Take(request.Filter.Take);

                var items = userQuery.ToList();

                var itemsDtos = _mapper.Map<List<UserResponse>>(items);

                IQueryable<dynamic> projectedQuery;
                if (request.Filter.SelectFields.Any())
                {
                    projectedQuery = userQuery.ApplyDynamicProjection(request.Filter.SelectFields);
                }
                else
                {
                    projectedQuery = userQuery.Select(r => (dynamic)r);
                }

                return Result<PagedResult<UserResponse>>.Success(new PagedResult<UserResponse>(itemsDtos,
                    totalCount,
                    request.Filter.PageNumber,
                    request.Filter.PageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result<PagedResult<UserResponse>>.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
