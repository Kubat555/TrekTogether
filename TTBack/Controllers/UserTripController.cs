using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTBack.Models;

namespace TTBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTripController : ControllerBase
    {
        private readonly TrekTogetherContext _context;

        public UserTripController(TrekTogetherContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserTrip>))]
        public IActionResult GetTrip()
        {
            if (_context.UserTrips == null)
            {
                return NotFound();
            }
            var trip = _context.UserTrips.OrderBy(user => user.UserId).ToList();

            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }
    }
}
