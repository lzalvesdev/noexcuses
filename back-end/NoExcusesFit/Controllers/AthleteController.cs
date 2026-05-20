using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NoExcusesFit.Domain.DTOs.Athlete;
using NoExcusesFit.Domain.Enums;
using NoExcusesFit.Domain.Interfaces.Business;
using System.ComponentModel.DataAnnotations;

namespace NoExcusesFit.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.Coach}")]
public class AthleteController : ControllerBase
{
    private readonly IAthleteBusiness _athleteBusiness;

    public AthleteController(IAthleteBusiness athleteBusiness)
    {
        _athleteBusiness = athleteBusiness;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery][Range(1, int.MaxValue)] int page = 1, [FromQuery][Range(1, int.MaxValue)] int pageSize = 10)
    {
        var athletes = await _athleteBusiness.GetAllAsync(page, pageSize);
        return Ok(athletes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var athlete = await _athleteBusiness.GetByIdAsync(id);
        return Ok(athlete);
    }

    [HttpPost("register")]
    [EnableRateLimiting("register")]
    [ProducesResponseType(typeof(AthleteResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterAthleteRequestDto request)
    {
        var athlete = await _athleteBusiness.RegisterAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = athlete.Id }, athlete);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateAthleteRequestDto request)
    {
        var athlete = await _athleteBusiness.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = athlete.Id}, athlete);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAthleteRequestDto request)
    {
        await _athleteBusiness.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _athleteBusiness.DeleteAsync(id);
        return NoContent();
    }
}