using TTBack.Models;

namespace TTBack.DTO
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsDriver { get; set; }
        public int? Rating { get; set; }
    }
}
