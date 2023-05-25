using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Services;

namespace HotelReservationSystem.API.Interfaces
{
    public interface IHotelRoomRepository
    {
        public bool SaveChanges();

        public IEnumerable<Room> GetRooms();
        public Room GetRoomById(int id);
        public void CreateRoom(Room room);
        public void UpdateRoom(Room room);
        public void DeleteRoom(int id);

        public (IEnumerable<Room> Rooms, PaginationMetadata PaginationMetadata) SearchRooms(int? minPrice, int? maxPrice, string? roomType, string? sortBy, bool ascending, int pageNumber, int pageSize);
    }
}
