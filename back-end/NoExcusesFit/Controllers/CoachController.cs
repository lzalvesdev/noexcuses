using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NoExcusesFit.Domain.DTOs.Coach;
using NoExcusesFit.Domain.Enums;
using NoExcusesFit.Domain.Interfaces.Business;
using System.ComponentModel.DataAnnotations;

namespace NoExcusesFit.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
public class CoachController : ControllerBase
{
    private readonly ICoachBusiness _coachBusiness;

    public CoachController(ICoachBusiness coachBusiness)
    {
        _coachBusiness = coachBusiness;
    }

    [HttpGet]
    [ProducesResponseType(typeof(CoachResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery][Range(1, int.MaxValue)] int page = 1, [FromQuery][Range(1, int.MaxValue)] int pageSize = 10)
    {
        var coaches = await _coachBusiness.GetAllAsync(page, pageSize);
        return Ok(coaches);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var coach = await _coachBusiness.GetByIdAsync(id);
        return Ok(coach);
    }

    [HttpPost("register")]
    [EnableRateLimiting("register")]
    [ProducesResponseType(typeof(CoachResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterCoachRequestDto request)
    {
        var coach = await _coachBusiness.RegisterAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = coach.Id }, coach);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCoachRequestDto request)
    {
        var coach = await _coachBusiness.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = coach.Id }, coach);
    }

    [HttpPost("{id}/speciality/{specialityId}")]
    public async Task<IActionResult> AddSpeciliaty([FromRoute] Guid id, [FromRoute] int specialityId)
    {
        await _coachBusiness.AddSpecialityAsync(id, specialityId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _coachBusiness.DeleteAsync(id);
        return NoContent();
    }
}