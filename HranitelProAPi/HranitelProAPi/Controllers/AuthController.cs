using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
                return BadRequest(new { message = "Некорректные данные" });

            var normalizedEmail = NormalizeEmail(dto.Email);
            if (normalizedEmail == null)
                return BadRequest(new { message = "Email обязателен" });

            var loweredEmail = normalizedEmail.ToLowerInvariant();
            var duplicateExists = await _context.Users.AnyAsync(u =>
                u.Email == normalizedEmail || u.Email.ToLower() == loweredEmail);
            if (duplicateExists)
                return BadRequest(new { message = "Пользователь с таким email уже существует" });

            var user = new User
            {
                FullName = dto.FullName,
                Email = normalizedEmail,
                PasswordHash = FormatBcryptHash(BCrypt.Net.BCrypt.HashPassword(dto.Password)),
                RoleId = dto.RoleId,
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
            if (dto == null)
                return Unauthorized(new { message = "Неверный email или пароль" });

            var normalizedEmail = NormalizeEmail(dto.Email);
            if (normalizedEmail == null)
                return Unauthorized(new { message = "Неверный email или пароль" });

            var loweredEmail = normalizedEmail.ToLowerInvariant();

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null)
            {
                user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == loweredEmail);
            }

            if (user == null)
                return Unauthorized(new { message = "Неверный email или пароль" });

            var passwordCheck = VerifyPassword(user.PasswordHash, dto.Password);
            if (!passwordCheck.IsValid)
                return Unauthorized(new { message = "Неверный email или пароль" });

            if (passwordCheck.ShouldUpgrade)
            {
                user.PasswordHash = FormatBcryptHash(BCrypt.Net.BCrypt.HashPassword(dto.Password));
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
                    Role = user.Role.Name
                }
            });
        }

        private static string? NormalizeEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return email.Trim();
        }

        private static (bool IsValid, bool ShouldUpgrade) VerifyPassword(string storedHash, string password)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
                return (false, false);

            if (string.IsNullOrEmpty(password))
                return (false, false);

            var normalizedHash = storedHash.Trim();
            var (algorithm, hashPayload) = ParseAlgorithm(normalizedHash);

            switch (algorithm)
            {
                case PasswordAlgorithm.Bcrypt:
                    {
                        var isValid = BCrypt.Net.BCrypt.Verify(password, hashPayload);
                        return (isValid, false);
                    }
                case PasswordAlgorithm.Sha256:
                    {
                        var computed = ComputeSha256Hex(password);
                        var isValid = string.Equals(computed, hashPayload, StringComparison.OrdinalIgnoreCase);
                        return (isValid, isValid);
                    }
                default:
                    return (false, false);
            }
        }

        private static string FormatBcryptHash(string bcryptHash) => $"BCRYPT::{bcryptHash}";

        private static (PasswordAlgorithm Algorithm, string Hash) ParseAlgorithm(string storedHash)
        {
            var hash = storedHash.Trim();

            if (hash.StartsWith("BCRYPT::", StringComparison.OrdinalIgnoreCase))
                return (PasswordAlgorithm.Bcrypt, hash.Substring("BCRYPT::".Length));

            if (hash.StartsWith("SHA256::", StringComparison.OrdinalIgnoreCase))
                return (PasswordAlgorithm.Sha256, hash.Substring("SHA256::".Length));

            if (hash.StartsWith("0x", StringComparison.OrdinalIgnoreCase) && hash.Length == 66 && IsSha256Hash(hash.Substring(2)))
                return (PasswordAlgorithm.Sha256, hash.Substring(2));

            if (hash.StartsWith("$2", StringComparison.Ordinal))
                return (PasswordAlgorithm.Bcrypt, hash);

            if (IsSha256Hash(hash))
                return (PasswordAlgorithm.Sha256, hash);

            return (PasswordAlgorithm.Unknown, hash);
        }

        private static bool IsSha256Hash(string hash) => hash.Length == 64 && hash.All(IsHexChar);

        private static bool IsHexChar(char c) =>
            (c >= '0' && c <= '9') ||
            (c >= 'a' && c <= 'f') ||
            (c >= 'A' && c <= 'F');

        private static string ComputeSha256Hex(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(bytes);
            return ConvertToHex(hashBytes);
        }

        private static string ConvertToHex(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
            }
            return builder.ToString();
        }

        private enum PasswordAlgorithm
        {
            Unknown,
            Bcrypt,
            Sha256
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
                new Claim("role", user.Role.Name)
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
