using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.API.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
