using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HranitelPRO.API.Contracts;
using HranitelPRO.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityWorkflowService _workflowService;

        public SecurityController(ISecurityWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<SecurityRequestDto>>> GetApprovedRequests([FromQuery] SecurityQuery query)
        {
            var requests = await _workflowService.GetApprovedRequestsAsync(query);
            return Ok(requests);
        }

        [HttpPost("requests/{id:int}/allow")]
        public async Task<ActionResult> AllowAccess(int id, [FromBody] SecurityAccessDto dto)
        {
            try
            {
                await _workflowService.AllowAccessAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("requests/{id:int}/complete")]
        public async Task<ActionResult> CompleteVisit(int id, [FromBody] SecurityCompleteDto dto)
        {
            try
            {
                var completed = await _workflowService.CompleteVisitAsync(id, dto);
                return Ok(new { completed });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
