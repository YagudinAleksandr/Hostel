using Hostel.Infrastructure.Entities.Base;

namespace Hostel.Infrastructure.Entities
{
    /// <summary>
    /// Базовый интерфейс сущности пользователя
    /// </summary>
    public interface IUserEntity : IBaseEntity
    {
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        string Fullname { get; set; }
        /// <summary>
        /// Является ли администратором
        /// </summary>
        bool IsAdmin { get; set; }
        /// <summary>
        /// Активен ли пользователь
        /// </summary>
        bool IsActive { get; set; }
        /// <summary>
        /// Изображение пользователя
        /// </summary>
        string ProfileImg { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        string Post { get; set; }
    }
}
