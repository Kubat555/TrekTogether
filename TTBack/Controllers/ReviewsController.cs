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
    public class ReviewsController : ControllerBase
    {
        private readonly TrekTogetherContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(TrekTogetherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public async Task<IActionResult> GetAllReviews()
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.ToListAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(reviews);
        }

        [HttpGet("getUserReviews/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetUserReviews(int userId)
        {
            if (_context.Reviews == null)
            {
                return BadRequest("Ошибка с базой данных при подключении!");
            }
            var reviews = _context.Reviews.Where(r => r.DriverId == userId).ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpPost("addReview")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest("Invalid data -> Отправленные данные неправильные или пустые");
            }
            var reviewCheck = await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == review.UserId && r.DriverId == review.DriverId);
            if (reviewCheck != null)
            {
                reviewCheck.Rating = review.Rating;
                reviewCheck.Comment = review.Comment;
            }
            else
            {
                _context.Reviews.Add(review);
            }

            await _context.SaveChangesAsync();

            var driver = await _context.Users.FindAsync(review.DriverId);
            if (driver != null)
            {
                // Вычислите новый рейтинг, учитывая новый отзыв
                var totalRating = _context.Reviews.Where(r => r.DriverId == review.DriverId).Select(r => r.Rating).Sum();
                var numberOfReviews = _context.Reviews.Count(r => r.DriverId == review.DriverId);
                var newRating = numberOfReviews > 0 ? totalRating / numberOfReviews : 0;

                // Обновление рейтинга водителя
                driver.Rating = newRating;

                // Сохранение изменений
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Server error: can't find driver Id");
            }

            return Ok("Review added successfully");
        }

    }
}
