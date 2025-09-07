using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : AggregateRoot<Guid>
    {
        /// <summary>
        /// Адрес жлектронной почты
        /// </summary>
        public EmailVo Email { get; private set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public FullNameVo Name { get; private set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswordHash { get; private set; }

        /// <summary>
        /// Статус пользователя
        /// </summary>
        public UserStatus Status { get; private set; }

        /// <summary>
        /// Тип пользователя
        /// </summary>
        public UserType Type { get; private set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        private User():base(Guid.Empty) 
        {
            Email = null!;
            PasswordHash = null!;
            Status = null!;
            Type = null!;
            Name = null!;
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="name">ФИО</param>
        /// <param name="passwordHash">Хэш пароля</param>
        /// <param name="type">Тип</param>
        /// <param name="status">Статус</param>
        /// <param name="createdAt">Дата создания</param>
        protected User(Guid id, EmailVo email, FullNameVo name, string passwordHash, UserType type, UserStatus status, DateTime createdAt) 
            : base(id)
        {
            Email = email;
            PasswordHash = passwordHash;
            Type = type;
            CreatedAt = createdAt;
            Status = status;
            Name = name;

            AddDomainEvent(new UserCreatedEvent(Id, Email.Value));
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <param name="name">ФИО</param>
        /// <param name="passwordHash">Хэш пароля</param>
        /// <param name="type">Тип пользователя <see cref="UserTypes"/></param>
        /// <param name="status">Статус активности пользователя <see cref="UserStatuses"/></param>
        /// <returns>Пользователь <see cref="User"/></returns>
        /// <exception cref="DomainRequiredFieldException"></exception>
        public static User Create(EmailVo email, FullNameVo name, string passwordHash, UserType type, UserStatus status)
        {
            if (string.IsNullOrEmpty(passwordHash))
                throw new DomainRequiredFieldException(ServicesUsersFieldCodes.ServicesUsersFieldPassword);

            return new User(Guid.NewGuid(),
                email,
                name,
                passwordHash,
                type,
                status,
                DateTime.UtcNow);
        }

        #region Logic

        /// <summary>
        /// Смена статуса
        /// </summary>
        /// <param name="status">Новый статус</param>
        public void ChangeStatus(UserStatus status)
        {
            if (Status.Equals(status))
                return;

            Status = status;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserChangedStatusEvent(Id, Status.DisplayName));
        }

        /// <summary>
        /// Смена типа
        /// </summary>
        /// <param name="type">Новый тип</param>
        public void ChangeType(UserType type)
        {
            if (Type.Equals(type))
                return;

            Type = type;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserChangedTypeEvent(Id, Type.DisplayName));
        }

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="passwordHash">Хэш нового пароля</param>
        /// <exception cref="DomainInactiveUserException"></exception>
        public void ChangePassword(string passwordHash)
        {
            if (passwordHash.Equals(PasswordHash))
                return;

            if (Status.Equals(UserStatuses.Inactive) || Status.Equals(UserStatuses.Blocked))
                throw new DomainInactiveUserException();

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Смена ФИО
        /// </summary>
        /// <param name="name">Новое ФИО</param>
        /// <exception cref="DomainInactiveUserException"></exception>
        public void ChangeName(FullNameVo name)
        {
            if (Status.Equals(UserStatuses.Inactive) || Status.Equals(UserStatuses.Blocked))
                throw new DomainInactiveUserException();

            if (Name.Equals(name))
                return;

            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Смена адреса электронной почты
        /// </summary>
        /// <param name="email">Новый адрес электронной почты</param>
        /// <exception cref="DomainInactiveUserException"></exception>
        public void ChangeEmail(EmailVo email)
        {
            if (Status.Equals(UserStatuses.Inactive) || Status.Equals(UserStatuses.Blocked))
                throw new DomainInactiveUserException();

            if (Email.Equals(email)) 
                return;

            Email = email;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserChangedEmailEvent(Id, Email.Value));
        }
        #endregion
    }
}
