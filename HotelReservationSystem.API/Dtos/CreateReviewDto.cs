namespace HotelReservationSystem.API.Dtos
{
    public class CreateReviewDto
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
    }
}
