﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTBack.Models;

namespace TTBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverTripController : ControllerBase
    {
        private readonly TrekTogetherContext _context;

        public DriverTripController(TrekTogetherContext context)
        {
            _context = context;
        }



        [HttpGet("driver/{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            if (_context.Trips == null)
            {
                return NotFound();
            }
            var trip = await _context.Trips.Where(x => x.DriverId == id).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(trip);
        }

    }
}
