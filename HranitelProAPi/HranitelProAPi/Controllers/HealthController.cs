using System;
using System.Threading.Tasks;
using HranitelPro.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HranitelContext _context;

        public HealthController(HranitelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns basic connectivity information for the API and database.
        /// </summary>
        [HttpGet("ping")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Ping()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                return Ok(new
                {
                    api = "ok",
                    database = canConnect,
                    timestamp = DateTimeOffset.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    api = "error",
                    database = false,
                    message = ex.Message,
                    timestamp = DateTimeOffset.UtcNow
                });
            }
        }
    }
}
