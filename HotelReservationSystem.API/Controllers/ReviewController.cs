using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IHotelReviewsRepository _reviewsRepository;
        private readonly IMapper _mapper;

        public ReviewsController(IHotelReviewsRepository reviewsRepository, IMapper mapper)
        {
            _reviewsRepository = reviewsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReviewDto>> GetAllReviews()
        {
            var reviews = _reviewsRepository.GetAllReviews();
            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return Ok(reviewDtos);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReviewDto>> GetReviewsByRoomId(int roomId)
        {
            var reviews = _reviewsRepository.GetReviewsByRoomId(roomId);

            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return Ok(reviewDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ReviewDto> GetReviewById(int id)
        {
            var review = _reviewsRepository.GetReviewById(id);

            if (review == null)
                return NotFound();

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return Ok(reviewDto);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            var review = _mapper.Map<Review>(createReviewDto);

            if (review == null)
                return BadRequest();

            _reviewsRepository.CreateReview(review);
            _reviewsRepository.SaveChanges();

            return Created("ReviewURI", new { review.Rating});
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "user")]
        public ActionResult UpdateReview(int id, [FromBody] UpdateReviewDto updateReviewDto)
        {
            var review = _mapper.Map<Review>(updateReviewDto);
            _reviewsRepository.UpdateReview(review);
            _reviewsRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public ActionResult DeleteReview(int id)
        {
            var review = _reviewsRepository.GetReviewById(id);

            if (review == null)
                return NotFound();

            _reviewsRepository.DeleteReview(id);

            return NoContent();
        }
    }

}
