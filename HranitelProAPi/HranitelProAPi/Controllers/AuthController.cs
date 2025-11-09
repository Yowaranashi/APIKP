using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HranitelPRO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HranitelContext _context;
        private readonly IConfiguration _config;

        public AuthController(HranitelContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.FullName))
            {
                return BadRequest(new { message = "Заполните все обязательные поля" });
            }

            var normalizedEmail = NormalizeEmail(dto.Email);
            if (string.IsNullOrEmpty(normalizedEmail))
            {
                return BadRequest(new { message = "Неверный формат email" });
            }

            var emailAlreadyExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == normalizedEmail);

            if (emailAlreadyExists)
            {
                return BadRequest(new { message = "Пользователь с таким email уже существует" });
            }

            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
            {
                role = await _context.Roles
                    .OrderBy(r => r.Id)
                    .FirstOrDefaultAsync(r => r.Name == "Посетитель");

                if (role == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Базовая роль посетителя не найдена" });
                }
            }

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = normalizedEmail,
                PasswordHash = HashPassword(dto.Password),
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Регистрация успешна" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            var normalizedEmail = NormalizeEmail(dto.Email);
            if (string.IsNullOrEmpty(normalizedEmail))
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

            if (user == null)
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            var passwordCheck = VerifyPassword(dto.Password, user.PasswordHash);
            if (!passwordCheck.IsValid)
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            var shouldSave = false;

            if (!string.Equals(user.Email, normalizedEmail, StringComparison.Ordinal))
            {
                user.Email = normalizedEmail;
                shouldSave = true;
            }

            if (passwordCheck.ShouldUpgrade)
            {
                user.PasswordHash = HashPassword(dto.Password);
                shouldSave = true;
            }

            if (shouldSave)
            {
                await _context.SaveChangesAsync();
            }

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    Role = user.Role?.Name
                }
            });
        }

        private static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            var normalized = password.Trim();
            var hash = BCrypt.Net.BCrypt.HashPassword(normalized);
            return $"BCRYPT::{hash}";
        }

        private static (bool IsValid, bool ShouldUpgrade) VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
            {
                return (false, false);
            }

            var trimmedHash = storedHash.Trim();

            if (trimmedHash.StartsWith("BCRYPT::", StringComparison.OrdinalIgnoreCase))
            {
                var bcryptHash = trimmedHash.Substring("BCRYPT::".Length);

                try
                {
                    var isValid = BCrypt.Net.BCrypt.Verify(password, bcryptHash);
                    return (isValid, false);
                }
                catch
                {
                    return (false, false);
                }
            }

            if (trimmedHash.StartsWith("SHA256::", StringComparison.OrdinalIgnoreCase))
            {
                var shaHash = trimmedHash.Substring("SHA256::".Length);
                var computed = ComputeSha256Hex(password);
                var isValid = string.Equals(computed, shaHash, StringComparison.OrdinalIgnoreCase);
                return (isValid, isValid);
            }

            if (LooksLikeSha256(trimmedHash))
            {
                var computed = ComputeSha256Hex(password);
                var isValid = string.Equals(computed, trimmedHash, StringComparison.OrdinalIgnoreCase);
                return (isValid, isValid);
            }

            return (false, false);
        }

        private static string? NormalizeEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var trimmed = email.Trim();

            try
            {
                var mailAddress = new MailAddress(trimmed);
                var localPart = mailAddress.User.Normalize();
                var domainPart = mailAddress.Host.Normalize();
                return $"{localPart}@{domainPart}".ToLowerInvariant();
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static bool LooksLikeSha256(string value)
        {
            if (value.Length != 64)
            {
                return false;
            }

            foreach (var ch in value)
            {
                var isDigit = ch >= '0' && ch <= '9';
                var isUpper = ch >= 'A' && ch <= 'F';
                var isLower = ch >= 'a' && ch <= 'f';
                if (!isDigit && !isUpper && !isLower)
                {
                    return false;
                }
            }

            return true;
        }

        private static string ComputeSha256Hex(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexString(hashBytes);
        }

        // 🔒 Генерация JWT токена
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role?.Name ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTO классы
    public class RegisterDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
