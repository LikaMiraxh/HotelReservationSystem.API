﻿namespace HotelReservationSystem.API.Dtos
{
    public class ReservationDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
    }
}
