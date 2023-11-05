using AutoMapper;
using TTBack.DTO;
using TTBack.Models;

namespace TTBack.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserTripDto, UserTrip>();
            CreateMap<Trip, TripDto>();
            CreateMap<Car, CarRegistrationDto>();
        }
    }
}
