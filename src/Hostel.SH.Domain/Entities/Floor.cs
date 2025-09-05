using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SH.Domain
{
    /// <summary>
    /// Этаж
    /// </summary>
    public class Floor : Entity<Guid>
    {
        /// <summary>
        /// Название
        /// </summary>
        public NameVo Name { get; private set; }

        /// <summary>
        /// Идентификатор подъезда <see cref="Entrance"/>
        /// </summary>
        private readonly Guid _entranceId;

        /// <summary>
        /// Этаж (EF Core)
        /// </summary>
        private Floor():base(Guid.Empty) 
        {
            Name = null!;
        }

        /// <summary>
        /// Этаж
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название</param>
        /// <param name="enterance">Подъезд <see cref="Entrance"/></param>
        internal Floor(Guid id, NameVo name, Entrance enterance) : base(id) 
        {
            Name = name;
            _entranceId = enterance.Id;
        }

        /// <summary>
        /// Обновление названия
        /// </summary>
        /// <param name="name">Название</param>
        public void UpdateName(NameVo name) 
        {
            Name = name;
        }
    }
}
