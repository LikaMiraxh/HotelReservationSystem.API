using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.API.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RoomId { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }
    }
}
