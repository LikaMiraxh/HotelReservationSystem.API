using HotelReservationSystem.API.Models;

namespace HotelReservationSystem.API.Interfaces
{
    public interface IHotelReviewsRepository
    {
        public bool SaveChanges();

        public IEnumerable<Review> GetReviewsByRoomId(int roomId);
        public Review GetReviewById(int reviewId);
        public void CreateReview(Review review);
        public void UpdateReview(Review review);
        public void DeleteReview(int reviewId);
    }
}
