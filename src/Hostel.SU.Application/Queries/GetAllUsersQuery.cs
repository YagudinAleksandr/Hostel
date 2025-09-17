using AutoMapper;
using Hostel.Shared.Application.Common;
using Hostel.Shared.Kernel;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Запрос на получение всех пользователей с фильтрацией
    /// </summary>
    /// <param name="Filter">Фильтр</param>
    public record GetAllUsersQuery(QueryFilter Filter) : IRequest<Result<IReadOnlyList<UserResponse>>>;

    /// <summary>
    /// Обработчик запроса на получение всех пользователей с фильтрацией
    /// </summary>
    internal class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<IReadOnlyList<UserResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUsersHandler> _logger;

        public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetAllUsersHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = request.Filter;
                var query = await _userRepository.GetAllAsync(cancellationToken);

                // Поиск по Email или Name (если задан Search)
                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    query = query.Where(u =>
                        u.Email.Value.Contains(filter.Search) ||
                        u.Name.ToString().Contains(filter.Search));
                }

                // Динамические фильтры
                if (filter.Filters != null)
                {
                    foreach (var kvp in filter.Filters)
                    {
                        if (kvp.Key.Equals("Status", StringComparison.OrdinalIgnoreCase))
                            query = query.Where(u => u.Status.DisplayName == kvp.Value);
                        else if (kvp.Key.Equals("Type", StringComparison.OrdinalIgnoreCase))
                            query = query.Where(u => u.Type.DisplayName == kvp.Value);
                    }
                }

                // Сортировка
                if (!string.IsNullOrWhiteSpace(filter.SortBy))
                {
                    var sortDirection = filter.SortDirection?.ToLower() == "desc" ? "descending" : "ascending";
                    query = query.OrderBy($"{filter.SortBy} {sortDirection}");
                }
                else
                {
                    query = query.OrderBy(u => u.CreatedAt);
                }

                // Пагинация
                var skip = (filter.Page - 1) * filter.PageSize;
                query = query.Skip(skip).Take(filter.PageSize);

                var users = query.ToList();
                var response = _mapper.Map<IReadOnlyList<UserResponse>>(users);
                return Result<IReadOnlyList<UserResponse>>.Success(response);
            }
            catch (DomainException dex)
            {
                return Result<IReadOnlyList<UserResponse>>.Failure(new Error(dex.ErrorCode, dex.Parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result<IReadOnlyList<UserResponse>>.Failure(new Error(DomainExceptionCodes.InternalServerError, [ex.Message]));
            }
        }
    }
}
