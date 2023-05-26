using AutoMapper;
using HotelReservationSystem.API.Dtos;
using HotelReservationSystem.API.Models;

namespace HotelReservationSystem.API.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();
        }
    }
}
