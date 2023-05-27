using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.API.Data;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Services;

namespace HotelReservationSystem.API.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly HotelContext _context;

        public HotelRoomRepository(HotelContext context)
        {
            _context = context;
        }

        public void CreateRoom(Room room)
        {
            _context.Rooms.Add(room);
            SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.Id == id);
            _context.Remove(room);
            SaveChanges();
        }

        public Room GetRoomById(int id)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.Id == id);
            return room;
        }

        public IEnumerable<Room> GetRooms()
        {
            var rooms = _context.Rooms.ToList();
            return rooms;
        }

        public (IEnumerable<Room>, PaginationMetadata) SearchRooms(int? minPrice, int? maxPrice, string? Type, string? sortBy, bool ascending, int pageNumber, int pageSize)
        {
            var rooms = _context.Rooms as IQueryable<Room>;

            if (minPrice != null)
            {
                rooms = rooms.Where(r => r.Price >= minPrice);
            }

            if (maxPrice != null)
            {
                rooms = rooms.Where(r => r.Price <= maxPrice);
            }

            if (!string.IsNullOrEmpty(Type))
            {
                rooms = rooms.Where(r => r.Type == Type);
            }

            switch (sortBy)
            {
                case "Type":
                    rooms = ascending ? rooms.OrderBy(r => r.Type) : rooms.OrderByDescending(r => r.Type);
                    break;
                case "Price":
                    rooms = ascending ? rooms.OrderBy(r => r.Price) : rooms.OrderByDescending(r => r.Price);
                    break;
                default:
                    rooms = ascending ? rooms.OrderBy(r => r.Id) : rooms.OrderByDescending(r => r.Id);
                    break;
            }

            var totalItemCount = rooms.Count();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            rooms = rooms
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return (rooms.ToList(), paginationMetadata);
        }

        public bool SaveChanges()
        {
            _context.SaveChanges();
            return true;
        }

        public void UpdateRoom(Room room)
        {
            _context.Update(room);
            SaveChanges();
        }
    }
}
