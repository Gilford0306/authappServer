using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using authappServer.Data;
using authappServer.Models;

namespace authappServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // Регистрация
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Проверьте, существует ли пользователь
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return BadRequest("Пользователь с таким именем уже существует.");
            }

            // Сохраните пользователя в базе данных
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Регистрация прошла успешно.");
        }

        // Авторизация
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
            {
                return Unauthorized("Неправильное имя пользователя или пароль.");
            }

            return Ok("Авторизация прошла успешно.");
        }
    }
}
