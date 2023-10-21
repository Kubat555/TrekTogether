namespace TTBack.DTO
{
    public class TripDto
    {
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public DateTime? DepartureData { get; set; }
        public int? Price { get; set; }
        public int? AvailableSeats { get; set; }
        public int? DriverId { get; set; }
    }
}
