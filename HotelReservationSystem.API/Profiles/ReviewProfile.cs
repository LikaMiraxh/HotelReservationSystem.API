using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Models;

namespace HotelReservationSystem.API.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>();
        }
    }
}
