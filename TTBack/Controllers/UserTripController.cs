using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTBack.DTO;
using TTBack.Interface;
using TTBack.Models;

namespace TTBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTripController : ControllerBase
    {
        private readonly TrekTogetherContext _context;
        private readonly IMapper _mapper;
        private readonly ITripService _tripService;

        public UserTripController(TrekTogetherContext context, IMapper mapper, ITripService tripService)
        {
            _context = context;
            _mapper = mapper;
            _tripService = tripService;
        }

        [HttpGet("getAllUserTrips")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserTrip>))]
        public IActionResult GetAllUserTrips()
        {
            if (_context.UserTrips == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }
            var trips = _context.UserTrips;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(trips);
        }


        // Получение по id пользователя
        [HttpGet("{UserId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserTrip>))]
        public IActionResult GetUserTrips(int UserId)
        {
            if (_context.UserTrips == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }
            var trips = _context.UserTrips.Where(t => t.UserId == UserId).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(trips);
        }


        [HttpGet("trip/{UserId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TripInfoDto>))]
        public IActionResult GetTripsInfo(int UserId)
        {
            if (_context.UserTrips == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }

            var trips = _context.UserTrips
             .AsNoTracking()
            .Where(t => t.UserId == UserId)
            .Include(t => t.Trip)
            .Include(t => t.Trip.Car)
            .Include(t => t.Trip.Driver)
            .ToList();

            var tripsInfo = trips.Select(t => new TripInfoDto
            {
                trip = t.Trip,
                users = _tripService.GetUsersOfTrip(t.Trip.Id, t.Trip.DriverId),
                car = t.Trip.Car,
                driver = _mapper.Map<UserDto>(t.Trip.Driver)
            }).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tripsInfo);
        }


        [HttpPost("addUserToTrip")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> AddUserToTrip([FromBody] UserTripDto userTripDto)
        {
            if (userTripDto == null)
            {
                return BadRequest("Invalid data");
            }

            // Найдем поездку, к которой был добавлен пассажир
            var trip = _context.Trips.FirstOrDefault(t => t.Id == userTripDto.TripId);

            if (trip != null && trip.AvailableSeats > 0)
            {
                // Уменьшим количество доступных мест на 1
                trip.AvailableSeats--;

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();


                if (await _context.UserTrips.AnyAsync(u => u.UserId == userTripDto.UserId && u.TripId == userTripDto.TripId))
                {
                    return BadRequest("Такая поездка у пользователя уже есть Шизик!");
                }

                var userTrip = _mapper.Map<UserTrip>(userTripDto);

                _context.UserTrips.Add(userTrip);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Место в поездке НЕ ОСТАЛОСЬ или поездка не найдена!");
            }


            return Ok("Пользователь успешно добавлен к поездке, работай Шизик!!");
        }

        [HttpDelete("deleteUserTrip/{userId}/{tripId}")]
        public async Task<IActionResult> DeleteUserTrip(int userId, int tripId)
        {
            try
            {
                if (_context.Trips == null)
                {
                    return NotFound("Trips table not found");
                }

                var userTrip = await _context.UserTrips
                    .Where(ut => ut.TripId == tripId && ut.UserId == userId)
                    .FirstOrDefaultAsync();

                if (userTrip == null)
                {
                    return NotFound("UserTrip not found");
                }

                _context.UserTrips.Remove(userTrip);
                await _context.SaveChangesAsync();

                return Ok("UserTrip deleted successfully");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return StatusCode(500, "Internal server error");
            }
        }
    }
 }