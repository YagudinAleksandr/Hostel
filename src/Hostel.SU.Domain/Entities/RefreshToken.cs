namespace Hostel.SU.Domain
{
    /// <summary>
    /// Токен обновления
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Дата истечения
        /// </summary>
        public DateTime Expires { get; private set; }

        /// <summary>
        /// Отозван
        /// </summary>
        public bool IsRevoked { get; private set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Токен обновления
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="expires">Дата истечения</param>
        /// <param name="userId">Идентификатор пользователя</param>
        public RefreshToken(string token, DateTime expires, Guid userId)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            IsRevoked = false;
        }

        /// <summary>
        /// Отозвать токен
        /// </summary>
        public void Revoke() => IsRevoked = true;

        /// <summary>
        /// Токен активен
        /// </summary>
        public bool IsActive => !IsRevoked && DateTime.UtcNow < Expires;
    }
}
