using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TTBack.Models
{
    public partial class Car
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? GosNomer { get; set; }
        public string? CarModel { get; set; }
        public string? CarYear { get; set; }

        public int? UserId { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
        [JsonIgnore]
        public ICollection<Trip>? Trips;
    }
}
