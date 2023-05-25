using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.API.Models
{
    public class Reservation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        
        public int CustomerId { get; set; }
      
        public Customer Customer { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

       
    }
}
