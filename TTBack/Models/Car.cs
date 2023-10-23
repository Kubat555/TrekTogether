using System;
using System.Collections.Generic;

namespace TTBack.Models
{
    public partial class Car
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CarMake { get; set; }
        public string? CarModel { get; set; }
        public string? CarYear { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public ICollection<Trip>? Trips { get; set; }
    }
}
