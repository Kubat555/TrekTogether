using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TTBack.DTO;
using TTBack.Interface;
using TTBack.Models;

namespace TTBack.Services
{
    public class TripService : ITripService
    {
        private readonly TrekTogetherContext _context;
        private readonly IMapper _mapper;

        public TripService(TrekTogetherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddNewTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToTripAsync(UserTrip userTrip)
        {
            _context.UserTrips.Add(userTrip);
            await _context.SaveChangesAsync();
        }

        public List<UserDto> GetUsersOfTrip(int tripId, int? driverId)
        {
            var users = _context.UserTrips.Where(t => t.TripId == tripId && t.UserId != driverId).Select(t => t.User).ToList();

            var result = new List<UserDto>();

            for (int i = 0; i < users.Count; i++)
            {
                result.Add(_mapper.Map<UserDto>(users[i]));
            }

            return result;
        }

        public async Task<bool> UserExists(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> CarExists(int carId)
        {
            return await _context.Cars.AnyAsync(c => c.Id == carId);
        }
    }
}
