using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoExcusesFit.Domain.DTOs.UserAccount;
using NoExcusesFit.Domain.Enums;
using NoExcusesFit.Domain.Interfaces.Business;
using System.ComponentModel.DataAnnotations;

namespace NoExcusesFit.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = Roles.Admin)]
public class UserAccountController : ControllerBase
{
    private readonly IUserAccountBusiness _userAccountBusiness;

    public UserAccountController(IUserAccountBusiness userAccountBusiness)
    {
        _userAccountBusiness = userAccountBusiness;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery][Range(1, int.MaxValue)] int page = 1, [FromQuery][Range(1, int.MaxValue)] int pageSize = 10)
    {
        var athletes = await _userAccountBusiness.GetAllAsync(page, pageSize);
        return Ok(athletes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var athletes = await _userAccountBusiness.GetByIdAsync(id);
        return Ok(athletes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserAccountRequestDto request)
    {
        await _userAccountBusiness.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _userAccountBusiness.DeleteAsync(id);
        return NoContent();
    }
}