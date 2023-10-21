using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTBack.DTO;
using TTBack.Models;

namespace TTBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TrekTogetherContext _context;
        private readonly IMapper _mapper;

        public TripController(TrekTogetherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Trip>))]
        public async Task<IActionResult> GetAllTrip()
        {
            if (_context.Trips == null)
            {
                return NotFound();
            }
            var trips = await _context.Trips.ToListAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(trips);
        }

        [HttpPost("addTrip")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> AddNewTrip([FromBody] TripDto TripDto)
        {
            if (TripDto == null)
            {
                return BadRequest("Invalid data");
            }

            var trip = new Trip
            {
                DepartureCity = TripDto.DepartureCity,
                ArrivalCity = TripDto.ArrivalCity,
                DepartureData = TripDto.DepartureData,
                Price = TripDto.Price,
                AvailableSeats = TripDto.AvailableSeats,
                DriverId = TripDto.DriverId 
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return Ok("Поездка успешно добавлена, работай Шизик!!");
        }

        [HttpGet("getUsers/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetOnlyTrips(int id)
        {
            if (_context.UserTrips == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }
            var users = _context.UserTrips.Where(t => t.TripId == id).Select(t => t.User).ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new List<UserDto>();

            for(int i = 0; i < users.Count; i++)
            {
                result.Add(_mapper.Map<UserDto>(users[i]));
            }

            return Ok(result);
        }
    }
}
