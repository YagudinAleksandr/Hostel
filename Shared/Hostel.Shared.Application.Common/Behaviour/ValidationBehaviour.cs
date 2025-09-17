using FluentValidation;
using MediatR;
using System.Reflection;

namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Поведение пайплайна MediatR, которое автоматически выполняет валидацию запроса с использованием FluentValidation.
    /// Если валидация не проходит, возвращается объект Result или Result<T> с ошибкой валидации.
    /// В случае успешной валидации запрос передаётся дальше по пайплайну.
    /// </summary>
    /// <typeparam name="TRequest">Тип входного запроса, реализующий интерфейс IRequest.</typeparam>
    /// <typeparam name="TResponse">Тип возвращаемого результата, ожидается Result или Result<T>.</typeparam>
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Асинхронно обрабатывает запрос, выполняя все зарегистрированные валидаторы для запроса.
        /// При наличии ошибок валидации формирует и возвращает ответ с ошибкой.
        /// В противном случае передаёт выполнение следующему обработчику в пайплайне.
        /// </summary>
        /// <param name="request">Входной запрос, который необходимо валидировать.</param>
        /// <param name="next">Делегат, вызывающий следующий обработчик в пайплайне.</param>
        /// <param name="cancellationToken">Токен для отмены операции.</param>
        /// <returns>
        /// Результат типа Result или Result<T> с ошибкой при проваленной валидации, 
        /// или результат выполнения следующего обработчика при успешной валидации.
        /// </returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                // Извлекаем сообщения об ошибках как строки
                var errorMessages = failures.Select(f => f.ErrorMessage).ToArray();

                // Создаём объект Error с кодом "Validation" и сообщениями как параметры
                var error = new Error("Validation", errorMessages);

                return CreateFailureResponse(error);
            }

            return await next();
        }

        /// <summary>
        /// Создаёт неуспешный результат (Failure) с указанной ошибкой.
        /// Поддерживает только <see cref="Result"/> и <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="error">Ошибка валидации.</param>
        /// <returns>Экземпляр <typeparamref name="TResponse"/> с ошибкой.</returns>
        /// <exception cref="InvalidOperationException">Если тип ответа не поддерживается.</exception>
        private TResponse CreateFailureResponse(Error error)
        {
            var responseType = typeof(TResponse);

            // Обработка: Result
            if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }

            // Обработка: Result<T>
            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(Result<>).MakeGenericType(responseType.GetGenericArguments()[0]);
                var failureMethod = resultType.GetMethod("Failure", BindingFlags.Public | BindingFlags.Static);
                if (failureMethod == null)
                    throw new InvalidOperationException($"Метод 'Failure' не найден в типе {resultType}.");

                var failureResult = failureMethod.Invoke(null, new object[] { error });
                return (TResponse)failureResult!;
            }

            throw new InvalidOperationException(
                $"Тип ответа '{responseType.Name}' не поддерживается в {nameof(ValidationBehaviour<TRequest, TResponse>)}. " +
                $"Ожидается 'Result' или 'Result<T>'."
            );
        }
    }
}
