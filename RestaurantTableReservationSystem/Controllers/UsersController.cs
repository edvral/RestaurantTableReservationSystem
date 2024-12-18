using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using RestaurantTableReservationSystem.Data;
using RestaurantTableReservationSystem.DTOs;
using RestaurantTableReservationSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantTableReservationSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly RestaurantReservationContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(RestaurantReservationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var userDto = new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.Role
            };

            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerDTO.Username);
            if (existingUser != null)
            {
                return Conflict("A user with this username already exists.");
            }

            if (!registerDTO.Email.Contains("@"))
            {
                return UnprocessableEntity("Email must contain an '@' symbol.");
            }

            var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDTO.Email);
            if (existingUserByEmail != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            string role = registerDTO.Username.ToLower() == "admin" ? "Admin" : "User";

            var user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = passwordHash,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, new { message = "User registered successfully." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDTO.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            SaveRefreshToken(user.UserId, refreshToken);  

            return Ok(new { token, expiration = DateTime.Now.AddHours(1), refreshToken });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null || storedToken.Revoked)
            {
                return BadRequest("Invalid refresh token.");
            }

            storedToken.Revoked = true;

            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User logged out successfully." });
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.Revoked);

            if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            storedToken.Revoked = true;

            var user = await _context.Users.FindAsync(storedToken.UserId);
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            var newTokenEntry = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.UserId,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            _context.RefreshTokens.Add(newTokenEntry);
            await _context.SaveChangesAsync();

            return Ok(new { token = newAccessToken, refreshToken = newRefreshToken });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private void SaveRefreshToken(int userId, string refreshToken)
        {
            var tokenEntry = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
            };

            _context.RefreshTokens.Add(tokenEntry);
            _context.SaveChanges();
        }
    }
}
