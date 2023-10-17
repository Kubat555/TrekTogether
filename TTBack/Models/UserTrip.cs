namespace TTBack.Models
{
    public class UserTrip
    {
        public int UserId { get; set; }
        public int TripId { get; set; }
        public User User { get; set; }
        public Trip Trip { get; set; }
    }
}
