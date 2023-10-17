using System;
using System.Collections.Generic;

namespace TTBack.Models
{
    public partial class Trip
    {
        public int Id { get; set; }
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public DateTime? DepartureData { get; set; }
        public int? Price { get; set; }
        public int? AvailableSeats { get; set; }
        public int? DriverId { get; set; }

        public ICollection<UserTrip> UserTrips { get; set; }
        public User? Driver;
    }
}
