using Hostel.Domain.Primitives;
using Hostel.Shared.Kernel;

namespace Hostel.SH.Domain
{
    /// <summary>
    /// Подъезд
    /// </summary>
    public class Entrance : Entity<Guid>
    {
        /// <summary>
        /// Название
        /// </summary>
        public NameVo Name { get;private set; }

        /// <summary>
        /// Идентификатор общежития
        /// </summary>
        private readonly Guid _buildingId;

        /// <summary>
        /// Этажи
        /// </summary>
        private readonly List<Floor> _floors = new();

        /// <summary>
        /// Список этажей
        /// </summary>
        public IReadOnlyCollection<Floor> Floors => _floors.AsReadOnly();

        /// <summary>
        /// Подъезд
        /// </summary>
        private Entrance():base(Guid.Empty)
        {
            Name = null!;
        }

        /// <summary>
        /// Подъезд
        /// </summary>
        /// <param name="id">Идентификатор подъезда</param>
        /// <param name="name">Название</param>
        /// <param name="building">Общежитие <see cref="Building"/></param>
        internal Entrance(Guid id, NameVo name, Building building) : base(id)
        {
            Name = name;
            _buildingId = building.Id;
        }

        /// <summary>
        /// Добавление этажа
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns>Этаж <see cref="Floor"/></returns>
        public Floor AddFloor(NameVo name)
        {
            var floor = new Floor(Guid.NewGuid(), name, this);

            _floors.Add(floor);

            return floor;
        }

        /// <summary>
        /// Удаление этажа
        /// </summary>
        /// <param name="floorId">Идентификатор этажа</param>
        /// <exception cref="DomainResourceNotFoundException"></exception>
        public void RemoveFloor(Guid floorId)
        {
            var floor = _floors.FirstOrDefault(f => f.Id == floorId)
                ?? throw new DomainResourceNotFoundException("Floor", "Id", floorId);
            _floors.Remove(floor);
        }
    }
}
