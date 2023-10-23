namespace TTBack.DTO
{
    public class SearchTripDto
    {
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureData { get; set; }
        public int AvailableSeats { get; set; }
    }
}
