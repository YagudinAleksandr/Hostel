using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Токен сброса пароля
    /// </summary>
    public class UserResetPasswordToken : AggregateRoot<Guid>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Статус
        /// </summary>
        public ResetPasswordStatus Status { get; private set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Дата использования
        /// </summary>
        public DateTime? UsedAt { get; private set; }

        /// <summary>
        /// Дата истечения
        /// </summary>
        public DateTime ExpiredAt { get; private set; }

        /// <summary>
        /// Токен сброса пароля
        /// </summary>
        private UserResetPasswordToken() : base(Guid.Empty)
        {
            Status = ResetPasswordStatuses.Expired;
        }

        /// <summary>
        /// Токен сброса пароля
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="createdAt">Дата создания</param>
        /// <param name="expiredAt">Дата истечения</param>
        protected UserResetPasswordToken(Guid id, Guid userId, DateTime createdAt, DateTime expiredAt) : base(id)
        {
            UserId = userId;
            CreatedAt = createdAt;
            Status = ResetPasswordStatuses.Pending;
            ExpiredAt = expiredAt;
        }

        /// <summary>
        /// Создание токена сброса пароля
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Токен сброса пароля <see cref="UserResetPasswordToken"/></returns>
        public static UserResetPasswordToken Create(Guid userId)
        {
            return new UserResetPasswordToken(Guid.NewGuid(),
                userId,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7));
        }

        /// <summary>
        /// Отметить токен как использованный
        /// </summary>
        /// <exception cref="DomainExpiredTokenException"></exception>
        /// <exception cref="DomainUsedTokenException"></exception>
        public void MarkAsUsed()
        {
            if (Status == ResetPasswordStatuses.Expired)
                throw new DomainExpiredTokenException();

            if (Status == ResetPasswordStatuses.Used)
                throw new DomainUsedTokenException();

            Status = ResetPasswordStatuses.Used;
            UsedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Отметить токен как истекший
        /// </summary>
        /// <exception cref="DomainUsedTokenException"></exception>
        public void MarkAsExpired()
        {
            if (Status == ResetPasswordStatuses.Used)
                throw new DomainUsedTokenException();

            Status = ResetPasswordStatuses.Expired;
        }
    }
}
