using TTBack.DTO;
using TTBack.Models;

namespace TTBack.Interface
{
    public interface ITripService
    {
        Task AddNewTripAsync(Trip trip);
        Task AddUserToTripAsync(UserTrip userTrip);
        List<UserDto> GetUsersOfTrip(int tripId, int? driverId);
        Task<bool> UserExists(int userId);
        Task<bool> CarExists(int carId);
    }
}
