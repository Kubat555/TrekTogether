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
    public class UserTripController : ControllerBase
    {
        private readonly TrekTogetherContext _context;
        private readonly IMapper _mapper;

        public UserTripController(TrekTogetherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Trip>))]
        public IActionResult GetOnlyTrips(int UserId)
        {
            if (_context.UserTrips == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }
            var trips = _context.UserTrips.Where(t => t.UserId == UserId).Select(t => t.Trip).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(trips);
        }


        [HttpPost("AddToTrip")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Register([FromBody] UserTripDto userTripDto)
        {
            if (userTripDto == null)
            {
                return BadRequest("Invalid data");
            }

            // Проверка наличия
            if (await _context.UserTrips.AnyAsync(u => u.UserId == userTripDto.UserId && u.TripId == userTripDto.TripId))
            {
                return BadRequest("Такая поездка уже есть Шизик!");
            }
            //var userTrip = _mapper.Map<UserTrip>(userTripDto);

            var userTrip2 = new UserTrip
            {
                UserId = userTripDto.UserId,
                TripId = userTripDto.TripId
            };

            _context.UserTrips.Add(userTrip2);
            await _context.SaveChangesAsync();


            //здесь должно быть изменение количества доступных мест в поездке

            return Ok("Поездка успешно добавлена, работай Шизик!!");
        }
    }
}