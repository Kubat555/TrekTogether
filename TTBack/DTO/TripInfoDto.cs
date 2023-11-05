using TTBack.Models;

namespace TTBack.DTO
{
    public class TripInfoDto
    {
        public Trip trip { get; set; }
        public ICollection<UserDto> users { get; set; }
        public Car car { get; set; }
        public UserDto driver { get; set; }
    }
}
