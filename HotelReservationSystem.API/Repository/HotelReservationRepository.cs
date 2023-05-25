using HotelReservationSystem.API.Data;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Services;

namespace HotelReservationSystem.API.Repository
{
    public class HotelReservationRepository : IHotelReservationRepository
    {
        private readonly HotelContext _context;

        public HotelReservationRepository(HotelContext context)
        {
            _context = context;
        }

        public void CreateReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            SaveChanges();
        }

        public void DeleteReservation(int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            _context.Reservations.Remove(reservation);
            SaveChanges();
        }

        public Reservation GetReservationById(int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            return reservation;
        }

        public IEnumerable<Reservation> GetReservationsByCustomerId(int customerId)
        {
            return _context.Reservations
                .Where(r => r.CustomerId == customerId)
                .ToList();
        }

        public (IEnumerable<Reservation>, PaginationMetadata) SearchReservations(DateTime? minDate, DateTime? maxDate, string? sortBy, bool ascending, int pageNumber, int pageSize)
        {
            var reservations = _context.Reservations.AsQueryable();

            if (minDate.HasValue)
            {
                reservations = reservations.Where(r => r.StartDate >= minDate);
            }

            if (maxDate.HasValue)
            {
                reservations = reservations.Where(r => r.EndDate <= maxDate);
            }

            switch (sortBy)
            {
                case "StartDate":
                    reservations = ascending ? reservations.OrderBy(r => r.StartDate) : reservations.OrderByDescending(r => r.StartDate);
                    break;
                case "EndDate":
                    reservations = ascending ? reservations.OrderBy(r => r.EndDate) : reservations.OrderByDescending(r => r.EndDate);
                    break;
                default:
                    reservations = ascending ? reservations.OrderBy(r => r.Id) : reservations.OrderByDescending(r => r.Id);
                    break;
            }

            var totalItemCount = reservations.Count();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            reservations = reservations
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return (reservations.ToList(), paginationMetadata);
        }

        public bool SaveChanges()
        {
            _context.SaveChanges();
            return true;
        }

        public void UpdateReservation(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            SaveChanges();
        }
    }
}
