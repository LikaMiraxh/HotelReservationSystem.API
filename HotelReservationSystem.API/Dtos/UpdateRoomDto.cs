namespace HotelReservationSystem.API.Dtos
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
