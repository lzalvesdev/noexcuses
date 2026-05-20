using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NoExcusesFit.Domain.DTOs.RefreshToken;
using NoExcusesFit.Domain.DTOs.UserAccount;
using NoExcusesFit.Domain.Interfaces.Business;

namespace NoExcusesFit.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAccountBusiness _userAccountBusiness;

    public AuthController(IUserAccountBusiness userAccountBusiness)
    {
        _userAccountBusiness = userAccountBusiness;
    }

    [HttpPost("register")]
    [EnableRateLimiting("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var userId = await _userAccountBusiness.Register(request);
        return Created(string.Empty, new { id = userId });
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _userAccountBusiness.Login(request);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [EnableRateLimiting("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var response = await _userAccountBusiness.RefreshTokenAsync(request.RefreshToken);
        return Ok(response);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        await _userAccountBusiness.LogoutAsync(request.RefreshToken);
        return NoContent();
    }
}