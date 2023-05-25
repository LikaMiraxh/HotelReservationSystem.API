using HotelReservationSystem.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace HotelReservationSystem.API.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {

        }

        public DbSet<Room> Rooms{ get; set; }
        public DbSet<Reservation> Reservations{ get; set; }
        public DbSet<Customer> Customers{ get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
