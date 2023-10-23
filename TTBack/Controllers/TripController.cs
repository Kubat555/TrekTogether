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
            var trips = await _context.Trips.Where(t => t.DepartureCity == st.DepartureCity && t.ArrivalCity == st.ArrivalCity && t.DepartureData.Value.Date == date && t.AvailableSeats >= st.AvailableSeats).Include(t => t.Driver).Include(t => t.Car).ToListAsync();
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
                    CarMake = trip.Car.CarMake,
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
                DriverId = TripDto.DriverId,
                CarId = TripDto.CarId
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return Ok("Поездка успешно добавлена, работай Шизик!!");
        }
    }
}
