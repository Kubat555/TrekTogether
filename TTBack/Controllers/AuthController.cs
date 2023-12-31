﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TTBack.DTO;
using TTBack.Models;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TrekTogetherContext _context; 
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public AuthController(TrekTogetherContext context, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    [HttpGet("getUserInfo/{userId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserInfoDto>))]
    public IActionResult GetUserInfo(int userId)
    {
        if (_context.Users == null)
        {
            return BadRequest("Ошибка с базой данных при подключении!");
        }
        var user = _context.Users.Where(r => r.Id == userId).FirstOrDefault();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = _mapper.Map<UserInfoDto>(user);

        return Ok(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
    {
        if (registrationDto == null)
        {
            return BadRequest("Invalid data");
        }

        // Проверка наличия пользователя с таким же именем (или другой уникальной информацией)
        if (await _context.Users.AnyAsync(u => u.Name == registrationDto.Name))
        {
            return BadRequest("Такой пользователь уже есть Шизик!");
        }

        var user = new User
        {
            Name = registrationDto.Name,
            Password = registrationDto.Password,
            PhoneNumber = registrationDto.PhoneNumber,
            IsDriver = registrationDto.IsDriver,
            Rating = registrationDto.Rating
        };

        // Хеширование пароля
        user.Password = _passwordHasher.HashPassword(user, user.Password);

        // Добавление пользователя в базу данных
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Пользователь успешно зареган, работай Шизик!!");
    }


    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return BadRequest("Invalid data");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == loginDto.Name);

        if (existingUser == null)
        {
            return BadRequest("Такого пользователя нет Шизик!");
        }

        // Проверка пароля
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, loginDto.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Success)
        {
            // Пароль верный, пользователь аутентифицирован
            return Ok(_mapper.Map<UserDto>(existingUser));
        }

        return BadRequest("Invalid password");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (_context.Users == null)
        {
            return NotFound("Таблица не найдена");
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok("Пользователь удалён!");
    }
}
