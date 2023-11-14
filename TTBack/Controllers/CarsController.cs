using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTBack.DTO;
using TTBack.Interface;
using TTBack.Models;
using TTBack.Services;

namespace TTBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly TrekTogetherContext _context;
        private readonly ITripService _tripService;

        public CarsController(TrekTogetherContext context, ITripService tripService)
        {
            _context = context;
            _tripService = tripService;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
          if (_context.Cars == null)
          {
              return NotFound();
          }
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
          if (_context.Cars == null)
          {
              return NotFound();
          }
            var car = _context.Cars.Where(c => c.Id == id).FirstOrDefault();

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> PostCar([FromBody] CarRegistrationDto car)
        {
            if (_context.Cars == null || car.UserId == null)
            {
                return BadRequest("'Cars' is null or UserId is null.");
            }

            if (!await _tripService.UserExists((int)car.UserId))
            {
                return NotFound("User ID not found");
            }

            var newCar = new Car()
            {
                Name = car.Name,
                GosNomer = car.GosNomer,
                CarModel = car.CarModel,
                CarYear = car.CarYear,
                UserId = car.UserId
            };

            _context.Cars.Add(newCar);
            await _context.SaveChangesAsync();

            // Создаем URL для нового ресурса (автомобиля)
            var url = Url.Action("GetCarById", new { id = newCar.Id });

            return Created(url, newCar);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                if (_context.Cars == null)
                {
                    return NotFound("Cars table not found");
                }

                var car = await _context.Cars.FindAsync(id);

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return Ok("Car deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        private bool CarExists(int id)
        {
            return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
