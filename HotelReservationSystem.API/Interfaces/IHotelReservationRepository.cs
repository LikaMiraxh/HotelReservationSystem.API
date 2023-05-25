using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Services;

namespace HotelReservationSystem.API.Interfaces
{
    public interface IHotelReservationRepository
    {
        public bool SaveChanges();

        public IEnumerable<Reservation> GetReservationsByCustomerId(int customerId);
        public Reservation GetReservationById(int reservationId);
        public void CreateReservation(Reservation reservation);
        public void UpdateReservation(Reservation reservation);
        public void DeleteReservation(int reservationId);

        public (IEnumerable<Reservation> Reservations, PaginationMetadata PaginationMetadata) SearchReservations(DateTime? minDate, DateTime? maxDate, string? sortBy, bool ascending, int pageNumber, int pageSize);
    }
}
