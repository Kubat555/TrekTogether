using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TTBack.DTO;
using TTBack.Interface;
using TTBack.Models;
using TTBack.Services;

namespace TTBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TrekTogetherContext _context;
        private readonly IMapper _mapper;
        private readonly ITripService _tripService;

        public TripController(TrekTogetherContext context, IMapper mapper, ITripService tripService)
        {
            _context = context;
            _mapper = mapper;
            _tripService = tripService;
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

        [HttpGet("getUsers/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetUserOnlyTrips(int id)
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

        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TripWithUserDto>))]
        public async Task<IActionResult> GetSearchTrip([FromBody] SearchTripDto st)
        {
            if (st == null)
            {
                return NotFound();
            }
            var date = st.DepartureData.Date;
            var trips = await _context.Trips.Where(t => t.DepartureCity == st.DepartureCity && t.ArrivalCity == st.ArrivalCity && t.DepartureData.Value.Date == date && t.AvailableSeats >= st.AvailableSeats && t.IsCompleted == false).Include(t => t.Driver).Include(t => t.Car).ToListAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = new List<TripWithUserDto>();
            foreach (var trip in trips)
            {
                result.Add(new TripWithUserDto()
                {
                    Id = trip.Id,
                    DepartureCity = trip.DepartureCity,
                    ArrivalCity = trip.ArrivalCity,
                    DepartureData = trip.DepartureData,
                    AvailableSeats = trip.AvailableSeats,
                    Price = trip.Price,
                    DriverId = trip.DriverId,
                    DriverName = trip.Driver.Name,
                    DriverPhoneNumber = trip.Driver.PhoneNumber,
                    DriverRating = trip.Driver.Rating,
                    CarId = trip.CarId,
                    CarName = trip.Car.Name,
                    CarGosNomer = trip.Car.GosNomer,
                    CarModel = trip.Car.CarModel,
                    CarYear = trip.Car.CarYear
                });
            }

            return Ok(result);
        }

        [HttpPost("addTrip")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> AddNewTrip([FromBody] TripDto TripDto)
        {
            if (TripDto == null || TripDto.DriverId == null || TripDto.CarId == null )
            {
                return BadRequest("Invalid data: Ты что то пропустил, может id водителя или машины");
            }

            if (!await _tripService.UserExists((int)TripDto.DriverId))
            {
                return NotFound("User ID not found");
            }

            if (!await _tripService.CarExists((int)TripDto.CarId))
            {
                return NotFound("Car ID not found");
            }

            var trip = new Trip
            {
                DepartureCity = TripDto.DepartureCity,
                ArrivalCity = TripDto.ArrivalCity,
                DepartureData = TripDto.DepartureData,
                Price = TripDto.Price,
                AvailableSeats = TripDto.AvailableSeats,
                DriverId = TripDto.DriverId,
                CarId = TripDto.CarId,
                IsCompleted = false
            };

            await _tripService.AddNewTripAsync(trip);

            var userTrip = new UserTrip
            {
                UserId = (int)trip.DriverId,
                TripId = trip.Id
            };

            await _tripService.AddUserToTripAsync(userTrip);

            return Ok("Поездка успешно добавлена, работай Шизик!!");
        }

        [HttpPost("completeTrip/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CompleteTrip(int id)
        {
            if (_context.Trips == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }
            var trip = _context.Trips.Where(t => t.Id == id).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            trip.IsCompleted = true;
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        [HttpDelete("deleteTrip/{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                if (_context.Trips == null)
                {
                    return NotFound("Trips table not found");
                }

                var trip = await _context.Trips.FindAsync(id);

                if (trip == null)
                {
                    return NotFound("Trip not found");
                }

                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();

                return Ok("Trip deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        
    }
}



