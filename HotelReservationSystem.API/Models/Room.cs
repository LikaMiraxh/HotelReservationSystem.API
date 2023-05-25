using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationSystem.API.Models
{
    public class Room
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, (double)decimal.MaxValue, ErrorMessage = "The price must be a positive value")]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
