using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Models;

namespace HotelReservationSystem.API.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
        }
    }
}
