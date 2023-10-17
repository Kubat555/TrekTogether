using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TTBack.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsDriver { get; set; }
        public string Password { get; set; }

        public ICollection<UserTrip> UserTrips { get; set; }

        public ICollection<Trip> Trips { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}
