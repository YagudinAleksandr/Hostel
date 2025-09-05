using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SH.Domain
{
    /// <summary>
    /// Общежитие
    /// </summary>
    public class Building : AggregateRoot<Guid>
    {
        /// <summary>
        /// Название
        /// </summary>
        public NameVo Name { get; private set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public AddressVo Address { get; private set; }

        /// <summary>
        /// Подъезды
        /// </summary>
        private readonly List<Entrance> _entrances = new();

        /// <summary>
        /// Список полъездов
        /// </summary>
        public IReadOnlyCollection<Entrance> Entrances => _entrances.AsReadOnly();

        /// <summary>
        /// Общежитие
        /// </summary>
        private Building() : base(Guid.Empty)
        {
            Name = null!;
            Address = null!;
        }

        /// <summary>
        /// Общежитие
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название</param>
        /// <param name="address">Адрес</param>
        protected Building(Guid id, NameVo name, AddressVo address) : base(id)
        {
            Name = name;
            Address = address;
        }

        /// <summary>
        /// Создание общежития
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="address">Адрес</param>
        /// <returns>Общежитие <see cref="Building"/></returns>
        public static Building Create(NameVo name, AddressVo address)
        {
            return new Building(Guid.NewGuid(), name, address);
        }

        /// <summary>
        /// Добавление подъезда
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns>Подъезд <see cref="Entrance"/></returns>
        public Entrance AddEntrance(NameVo name)
        {
            var entrance = new Entrance(Guid.NewGuid(), name, this);
            _entrances.Add(entrance);
            return entrance;
        }

        /// <summary>
        /// Удаление подъезда
        /// </summary>
        /// <param name="entranceId">Идентификатор подъезда</param>
        /// <exception cref="DomainException"></exception>
        public void RemoveEntrance(Guid entranceId)
        {
            var entrance = _entrances.FirstOrDefault(e => e.Id == entranceId)
                ?? throw new DomainResourceNotFoundException("Entrance", "Id", entranceId);
            
            _entrances.Remove(entrance);
        }

        /// <summary>
        /// Обновление общежития
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="address">Адрес</param>
        public void Update(NameVo name, AddressVo address)
        {
            Name = name;
            Address = address;
        }
    }
}
