using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoExcusesFit.Domain.DTOs.Speciality;
using NoExcusesFit.Domain.Enums;
using NoExcusesFit.Domain.Interfaces.Business;

namespace NoExcusesFit.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.Coach}")]
public class SpecialityController : ControllerBase
{
    private readonly ISpecialityBusiness _specialityBusiness;

    public SpecialityController(ISpecialityBusiness specialityBusiness)
    {
        _specialityBusiness = specialityBusiness;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var speciality = await _specialityBusiness.GetAllAsync();
        return Ok(speciality);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateSpecialityRequestDto request)
    {
        var speciality = await _specialityBusiness.AddAsync(request);
        return Created(string.Empty, new { id = speciality.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSpecialityRequestDto request)
    {
        await _specialityBusiness.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _specialityBusiness.DeleteAsync(id);
        return NoContent();
    }
}