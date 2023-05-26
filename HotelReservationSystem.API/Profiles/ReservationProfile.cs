using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Models;

namespace HotelReservationSystem.API.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationDto>();
            CreateMap<CreateReservationDto, Reservation>();
            CreateMap<UpdateReservationDto, Reservation>();
        }
    }

}
