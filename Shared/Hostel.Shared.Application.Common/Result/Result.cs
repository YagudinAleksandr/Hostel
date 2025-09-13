namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Представляет результат операции: успех или ошибка.
    /// Используется для управления потоком выполнения без исключений.
    /// </summary>
    public abstract record Result
    {
        /// <summary>
        /// Указывает, что операция завершилась успешно.
        /// </summary>
        public bool IsSuccess { get; init; }

        /// <summary>
        /// Указывает, что операция завершилась с ошибкой.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Описание ошибки, если операция не удалась.
        /// При успехе содержит <see cref="Error.None"/>.
        /// </summary>
        public Error Error { get; init; } = null!;

        /// <summary>
        /// Инициализирует новый экземпляр результата.
        /// </summary>
        /// <param name="isSuccess">Признак успешного выполнения.</param>
        /// <param name="error">Ошибка, если операция не удалась; должна быть <see cref="Error.None"/> при успехе.</param>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если результат помечен как успех, но передана ошибка, 
        /// или как ошибка, но ошибка не указана.
        /// </exception>
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && !error.IsNone)
                throw new InvalidOperationException("Результат с успехом не может содержать ошибку.");
            if (!isSuccess && error.IsNone)
                throw new InvalidOperationException("Результат с ошибкой должен содержать описание ошибки.");

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Создаёт успешный результат.
        /// </summary>
        /// <returns>Успешный результат без значения.</returns>
        public static Result Success() => new SuccessResult();

        /// <summary>
        /// Создаёт результат с ошибкой.
        /// </summary>
        /// <param name="error">Описание ошибки.</param>
        /// <returns>Результат с ошибкой.</returns>
        public static Result Failure(Error error) => new FailureResult(error);

        /// <summary>
        /// Внутренний тип: успешный результат.
        /// </summary>
        private sealed record SuccessResult : Result
        {
            /// <summary>
            /// Инициализирует успешный результат.
            /// </summary>
            public SuccessResult() : base(true, Error.None) { }
        }

        /// <summary>
        /// Внутренний тип: результат с ошибкой.
        /// </summary>
        private sealed record FailureResult : Result
        {
            /// <summary>
            /// Инициализирует результат с ошибкой.
            /// </summary>
            /// <param name="error">Описание ошибки.</param>
            public FailureResult(Error error) : base(false, error) { }
        }
    }

    /// <summary>
    /// Представляет результат операции, возвращающей значение типа <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
    public abstract record Result<T> : Result
    {
        private readonly T? _value;

        /// <summary>
        /// Инициализирует новый экземпляр результата с значением.
        /// </summary>
        /// <param name="isSuccess">Признак успеха.</param>
        /// <param name="value">Возвращаемое значение (может быть null).</param>
        /// <param name="error">Ошибка, если операция не удалась.</param>
        protected Result(bool isSuccess, T? value, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// Возвращает значение результата, если операция прошла успешно.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается при попытке получить значение из неуспешного результата.
        /// </exception>
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Нельзя получить значение из неуспешного результата.");

        /// <summary>
        /// Создаёт успешный результат с указанным значением.
        /// </summary>
        /// <param name="value">Возвращаемое значение.</param>
        /// <returns>Успешный результат с значением.</returns>
        public static Result<T> Success(T value) => new SuccessResult<T>(value);

        /// <summary>
        /// Создаёт результат с ошибкой.
        /// </summary>
        /// <param name="error">Описание ошибки.</param>
        /// <returns>Результат с ошибкой.</returns>
        public static Result<T> Failure(Error error) => new FailureResult<T>(error);

        /// <summary>
        /// Внутренний тип: успешный результат с значением.
        /// </summary>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        private sealed record SuccessResult<TValue> : Result<TValue>
        {
            /// <summary>
            /// Инициализирует успешный результат с значением.
            /// </summary>
            /// <param name="value">Значение результата.</param>
            public SuccessResult(TValue value) : base(true, value, Error.None) { }
        }

        /// <summary>
        /// Внутренний тип: результат с ошибкой.
        /// </summary>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        private sealed record FailureResult<TValue> : Result<TValue>
        {
            /// <summary>
            /// Инициализирует результат с ошибкой.
            /// </summary>
            /// <param name="error">Описание ошибки.</param>
            public FailureResult(Error error) : base(false, default(TValue), error) { }
        }
    }
}
