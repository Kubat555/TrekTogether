namespace TTBack.DTO
{
    public class TripWithUserDto
    {
        public int Id { get; set; }
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public DateTime? DepartureData { get; set; }
        public int? Price { get; set; }
        public int? AvailableSeats { get; set; }
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public int? DriverRating { get; set; }
        public int? CarId { get; set; }
        public string? CarName { get; set; }
        public string? CarMake { get; set; }
        public string? CarModel { get; set; }
        public string? CarYear { get; set; }
    }
}
