using HotelReservationSystem.API.Data;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;

namespace HotelReservationSystem.API.Repository
{
    public class HotelReviewsRepository : IHotelReviewsRepository
    {
        private readonly HotelContext _context;

        public HotelReviewsRepository(HotelContext context)
        {
            _context = context;
        }

        public void CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            SaveChanges();
        }

        public void DeleteReview(int reviewId)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            _context.Reviews.Remove(review);
            SaveChanges();
        }

        public Review GetReviewById(int reviewId)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            return review;
        }

        public IEnumerable<Review> GetReviewsByRoomId(int roomId)
        {
            return _context.Reviews
                .Where(r => r.RoomId == roomId)
                .ToList();
        }

        public bool SaveChanges()
        {
            _context.SaveChanges();
            return true; ;
        }

        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            SaveChanges();
        }
    }
}
